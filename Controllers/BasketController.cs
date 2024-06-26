using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Context;
using Pronia.Models;
using Pronia.Models.ViewModels;
using System.Collections.Generic;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext context;

        public BasketController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult>  Add(int id)
        {
            if (id <= 0) return BadRequest();
             Product? product = await context.Products.FirstOrDefaultAsync(p=> p.Id == id);
            if(product==null) return NotFound();
            string jsonBasket = Request.Cookies["Basket"];
            ICollection<CookieBasketItemVM> basket;
            if (jsonBasket is not null)
            {

                basket = JsonConvert.DeserializeObject<ICollection<CookieBasketItemVM>>(jsonBasket);
               CookieBasketItemVM? existed = basket.FirstOrDefault(bi=>bi.Id==id);
                if (existed!=null)
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
              basket= new List<CookieBasketItemVM> { new CookieBasketItemVM { Id = id, Count = 1 } };
            }
            string serializedBasket =JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", serializedBasket);


           

          return RedirectToAction(nameof(Index),"Home");
        }
        public IActionResult GetCookie()
        {
            string json = Request.Cookies["Basket"];
           
            return Content(json);
        }
    }
}
