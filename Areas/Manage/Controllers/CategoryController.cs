using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext context;

        public CategoryController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult>Index()
        {
            IEnumerable<Category> categories = await context.Categories.Include(c => c.Products).ToListAsync();
            return View(categories);
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid) { return View(category); }
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
