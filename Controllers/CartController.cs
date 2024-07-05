using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;
using Pronia.Models.ViewModels;
using System.Security.Claims;

namespace Pronia.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext context;

        public CartController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task< IActionResult> Index()
        {
            ICollection<BasketItemVm> basketItems = new List<BasketItemVm>();
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<BasketItem> dbBasketItems = await context.BasketItems
                    .Where(bi => bi.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .Include(bi => bi.Product).ThenInclude(p => p.Images.Where(i => i.Type == ImageType.Main))
                    .ToListAsync();
                foreach (BasketItem bi in dbBasketItems)
                {
                    basketItems.Add(new BasketItemVm
                    {
                        Id = bi.Product.Id,
                        Name = bi.Product.Name,
                        Image = bi.Product.Images.FirstOrDefault()!.Url,
                        Price = bi.Product.Price,
                        Total = bi.Product.Price * bi.Count,
                        Count = bi.Count

                    });
                }
            }
            else
            {
                string? cookie = Request.Cookies["Basket"];
                if (cookie != null)
                {
                    ICollection<CookieBasketItemVM>? basket = JsonConvert.DeserializeObject<ICollection<CookieBasketItemVM>>(cookie);
                    if (basket != null)
                    {
                        foreach (CookieBasketItemVM cookieItem in basket)
                        {
                            Product? product = await context.Products
                                .Include(p => p.Images.Where(i => i.Type == ImageType.Main))
                                .FirstOrDefaultAsync(p => p.Id == cookieItem.Id);
                            if (product != null)
                            {
                                basketItems.Add(new BasketItemVm
                                {
                                    Id = product.Id,
                                    Name = product.Name,
                                    Image = product.Images.FirstOrDefault()!.Url,
                                    Price = product.Price,
                                    Total = product.Price * cookieItem.Count,
                                    Count = cookieItem.Count

                                });

                            }
                        }

                    }
                }
            }
            
            return View(basketItems);
        }
        
        public IActionResult RemoveItem(int itemId)
        {
            string? cookieValue = Request.Cookies["Basket"];

            if (cookieValue != null)
            {
                ICollection<CookieBasketItemVM>? basketItems = JsonConvert.DeserializeObject<ICollection<CookieBasketItemVM>>(cookieValue);

                if (basketItems != null)
                {
                    var itemToRemove = basketItems.FirstOrDefault(x => x.Id == itemId);
                    if (itemToRemove != null)
                    {
                        basketItems.Remove(itemToRemove);

                      
                        string updatedCookieValue = JsonConvert.SerializeObject(basketItems);
                        Response.Cookies.Append("Basket", updatedCookieValue);
                    }
                }
            }

            return RedirectToAction("Index");
        }

    }
}
