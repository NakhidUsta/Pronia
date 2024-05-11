using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
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


            //IEnumerable<Slide> slides = new List<Slide>
            //{
            //    new Slide
            //    {

            //        Title="65% Off",
            //        SubTitle="New Plant",
            //        Description="Pronia, With 100% Natural, Organic & Plant Shop.",
            //        ButtonText="Discover Now",
            //        Image="1-1-524x617.png",
            //        Order=4

            //    },
            //     new Slide
            //    {

            //        Title="55% Off",
            //        SubTitle="New Plant",
            //        Description="Pronia, With 100% Natural, Organic & Plant Shop.",
            //        ButtonText="Discover Now",
            //        Image="1-2-524x617.png",
            //        Order=3

            //    },
            //      new Slide
            //    {

            //        Title="45% Off",
            //        SubTitle="New Plant",
            //        Description="Pronia, With 100% Natural, Organic & Plant Shop.",
            //        ButtonText="Discover Now",
            //        Image="inner-image6.png",
            //        Order=2

            //    }
            // };
            //IEnumerable<Product> products = new List<Product>
            //{
            //    new Product
            //    {
            //        Name="American Marigold",
            //        Price=23.45m,
            //        ImagePrimary="1-10-270x300.jpg",
            //        ImageSecondary="1-11-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Black Eyed Susan",
            //        Price=26.45m,
            //        ImagePrimary="1-11-270x300.jpg",
            //        ImageSecondary="1-4-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Bleeding Heart",
            //        Price=30.55m,
            //        ImagePrimary="1-7-270x300.jpg",
            //        ImageSecondary="1-8-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Bloody Cranesbill",
            //        Price=45m,
            //        ImagePrimary="1-5-270x300.jpg",
            //        ImageSecondary="1-6-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="ButterFly Weed",
            //        Price=30m,
            //        ImagePrimary="1-4-270x300.jpg",
            //        ImageSecondary="1-11-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Common Yarrow",
            //        Price=23.45m,
            //        ImagePrimary="1-6-270x300.jpg",
            //        ImageSecondary="1-3-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Doublefile Viburnum",
            //        Price=67.45m,
            //        ImagePrimary="1-3-270x300.jpg",
            //        ImageSecondary="1-9-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Feather Reed Grass",
            //        Price=20m,
            //        ImagePrimary="1-3-270x300.jpg",
            //        ImageSecondary="1-4-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Doublefile Rose",
            //        Price=52m,
            //        ImagePrimary="1-1-270x300.jpg",
            //        ImageSecondary="1-2-270x300.jpg"

            //    },
            //    new Product
            //    {
            //        Name="Feather Green Grass",
            //        Price=28m,
            //        ImagePrimary="1-7-270x300.jpg",
            //        ImageSecondary="1-5-270x300.jpg"

            //    },
            //};
            //await context.AddRangeAsync(products);
            //await context.SaveChangesAsync();

            IEnumerable<Slide> slides= await context.Slides.ToListAsync();
            IEnumerable<Product> productsFromDbLatest = await context.Products.OrderByDescending(p => p.Id).Take(8).ToListAsync();
            IEnumerable<Product> productsFromDbFeatured = await context.Products.Take(8).ToListAsync();
            IEnumerable<Product> productsFromDbCheapest = await context.Products.OrderBy(p => p.Price).Take(8).ToListAsync();


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
