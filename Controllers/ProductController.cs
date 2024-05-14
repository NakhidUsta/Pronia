using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;
using Pronia.Models.ViewModels;

namespace Pronia.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext context;
        public ProductController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult>  Detail(int id)
        {
            if (id <= 0) return BadRequest();
           Product? product= await context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Images)
                .Include(p=>p.ProductTags).ThenInclude(pt=>pt.Tag)
                .FirstOrDefaultAsync(p=> p.Id == id);
            
            if (product == null) return NotFound();
            ICollection<Product> related = await context.Products
                .Include(p=>p.Images.Where(pi=>pi.Type!=ImageType.Additional))
                .Include(p=>p.Category)
                .Where(p=>p.Category.Name==product.Category.Name && product.Id!=p.Id).
                ToListAsync();


            ProductViewModel vm = new ProductViewModel
            {
                ProductDetail=product,
                ProductRelated = related

            };

            return View(vm);
        }
    }
}
