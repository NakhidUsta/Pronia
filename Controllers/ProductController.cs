using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Models;

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
           Product? product= await context.Products.Include(p=>p.Images).FirstOrDefaultAsync(p=> p.Id == id);

            if (product == null) return NotFound();


            return View(product);
        }
    }
}
