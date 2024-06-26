using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Models;
using Pronia.Models.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController:Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }
        
        public async Task<IActionResult> Index()
        {


           

            IEnumerable<Slide> slides= await context.Slides.ToListAsync();
            IEnumerable<Product> productsFromDbLatest = await context.Products.Include(p=>p.Images.Where(pi=>pi.Type!=ImageType.Additional)).OrderByDescending(p => p.Id).Take(8).ToListAsync();
            IEnumerable<Product> productsFromDbFeatured = await context.Products.Include(p => p.Images.Where(pi => pi.Type != ImageType.Additional)).Take(8).ToListAsync();
            IEnumerable<Product> productsFromDbCheapest = await context.Products.Include(p => p.Images.Where(pi => pi.Type != ImageType.Additional)).OrderBy(p => p.Price).Take(8).ToListAsync();


            HomeViewModel vm = new HomeViewModel
            {
                Slides = slides,
                ProductsCheapest=productsFromDbCheapest,
                ProductsFeatured=productsFromDbFeatured,
                ProductsLatest=productsFromDbLatest
                
            };
            return View(vm);
        }
    }
}
