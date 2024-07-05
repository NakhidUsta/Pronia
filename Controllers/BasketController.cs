using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Context;
using Pronia.Models;
using Pronia.Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppUser> userManager;

        public BasketController(AppDbContext context,UserManager<AppUser>userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<IActionResult>  Add(int id)
        {
            if (id <= 0) return BadRequest();
             Product? product = await context.Products.FirstOrDefaultAsync(p=> p.Id == id);
            if(product==null) return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await userManager.Users
                    .Where(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .Include(u=>u.BasketItems)
                    .FirstOrDefaultAsync();
                if(user==null) return NotFound();
                BasketItem? basketItem = user.BasketItems.FirstOrDefault(b => b.ProductId == id);
                if (basketItem is null)
                {
                    user.BasketItems.Add(new BasketItem { Count = 1, ProductId = product.Id });
                }
                else basketItem.Count++;
                await context.SaveChangesAsync();
            }
            else
            {
                string jsonBasket = Request.Cookies["Basket"];
                ICollection<CookieBasketItemVM>? basket;
                if (jsonBasket is not null)
                {

                    basket = JsonConvert.DeserializeObject<ICollection<CookieBasketItemVM>>(jsonBasket);
                    CookieBasketItemVM? existed = basket.FirstOrDefault(bi => bi.Id == id);
                    if (existed != null)
                    {
                        existed.Count++;
                    }
                    else
                    {

                        basket.Add(new CookieBasketItemVM
                        {
                            Id = id,
                            Count = 1
                        });
                    }
                }
                else
                {
                    basket = new List<CookieBasketItemVM> { new CookieBasketItemVM { Id = id, Count = 1 } };
                }
                string serializedBasket = JsonConvert.SerializeObject(basket);
                Response.Cookies.Append("Basket", serializedBasket);

            }






            return RedirectToAction(nameof(Index),"Home");
        }
       
    }
}
