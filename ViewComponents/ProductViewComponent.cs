using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;

namespace Pronia.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public ProductViewComponent(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(OrderEnum order)
        {
            IEnumerable<Product> products = new List<Product>();
            switch (order)
            {
                case OrderEnum.Featured:
                   products= await context.Products
                        .Include(p => p.Images.Where(pi => pi.Type != ImageType.Additional))
                        .ToListAsync();
                    break;
                case OrderEnum.Premium:
                  products=  await context.Products.OrderByDescending(p=>p.Price)
                        .Include(p => p.Images.Where(pi => pi.Type != ImageType.Additional))
                        .ToListAsync();
                    break;
                case OrderEnum.Latest:
                   products= await context.Products.OrderByDescending(p => p.Id)
                        .Include(p => p.Images.Where(pi => pi.Type != ImageType.Additional))
                        .ToListAsync();
                    break;
              
            }



            return View(products);
        }
    }
}
