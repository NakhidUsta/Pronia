using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext context;

        public ProductController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult>  Index()
        {
            IEnumerable<Product> products = await context.Products
                .Include(p=>p.Images.Where(pi=>pi.Type == ImageType.Main))
                .Include(p=>p.Category).ToListAsync();
            return View(products);
        }
    }
}
