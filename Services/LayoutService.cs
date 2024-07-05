using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;
using Pronia.Models.ViewModels;
using System.Security.Claims;

namespace Pronia.Services
{
    public class LayoutService
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor http;

        public LayoutService(AppDbContext context,IHttpContextAccessor http)
        {
            this.context = context;
            this.http = http;
        }
        public async Task<Dictionary<string,string>> GetSettingsAsync()
        {
            return await context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
        }
        public async Task<ICollection<BasketItemVm>> GetBasketAsync()
        {
            ICollection<BasketItemVm> basketItems = new List<BasketItemVm>();
            if (http.HttpContext.User.Identity.IsAuthenticated)
            {
                IEnumerable<BasketItem> dbBasketItems = await context.BasketItems
                    .Where(bi => bi.AppUserId == http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .Include(bi=>bi.Product).ThenInclude(p=>p.Images.Where(i=>i.Type==ImageType.Main))
                    .ToListAsync();
                foreach (BasketItem bi in dbBasketItems)
                {
                    basketItems.Add(new BasketItemVm
                    {
                        Id = bi.Product.Id,
                        Name = bi.Product.Name,
                        Image =bi.Product.Images.FirstOrDefault()!.Url,
                        Price = bi.Product.Price,
                        Total = bi.Product.Price * bi.Count,
                        Count =bi.Count

                    });
                }
            }
            else
            {
                string? cookie = http.HttpContext.Request.Cookies["Basket"];
                ICollection<CookieBasketItemVM> removeable = new List<CookieBasketItemVM>();
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
                            else removeable.Add(cookieItem);

                        }
                        if (removeable.Count > 0)
                        {
                            foreach (var item in removeable)
                            {
                                basket.Remove(item);
                            }
                            string jsonBasket = JsonConvert.SerializeObject(basket);
                            http.HttpContext.Response.Cookies.Append("Basket", jsonBasket);
                        }
                    }
                }
            }
            return basketItems;
        }
    }
}
