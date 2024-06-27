using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels;
using Pronia.Context;
using Pronia.Extencions;
using Pronia.Extencions.Enums;
using Pronia.Models;
using System;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult>  Index()
        {
            IEnumerable<Product> products = await context.Products
                .Include(p=>p.Images.Where(pi=>pi.Type == ImageType.Main))
                .Include(p=>p.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult>  Create()
        {
            IEnumerable<Category> categories = await context.Categories.ToListAsync();
            IEnumerable<Tag> tags = await context.Tags.ToListAsync();
            IEnumerable<Color> colors = await context.Colors.ToListAsync();
            return View(new ProductCreateVM { Categories = categories,Tags=tags,Colors=colors });
          
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                IEnumerable<Color> colors = await context.Colors.ToListAsync();
                vm.Colors=colors;
                vm.Categories=categories;
                vm.Tags=tags;
                return View(vm);
            }
            if (!vm.MainPhoto.CheckFileType(FileType.Image))
            {
                ModelState.AddModelError("MainPhoto", "Please Upload Image");
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                IEnumerable<Color> colors = await context.Colors.ToListAsync();
                vm.Colors = colors;
                vm.Categories = categories;
                vm.Tags = tags;
                return View(vm);
            }
            if (!vm.HoverPhoto.CheckFileType(FileType.Image))
            {
                ModelState.AddModelError("HoverPhoto", "Please Upload Image");
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                IEnumerable<Color> colors = await context.Colors.ToListAsync();
                vm.Colors = colors;
                vm.Categories = categories;
                vm.Tags = tags;
                return View(vm);
            }
            if (!vm.MainPhoto.CheckFileSize(2, FileSize.Mb))
            {
                ModelState.AddModelError("Photo", "Invalid Photo Size");
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                IEnumerable<Color> colors = await context.Colors.ToListAsync();
                vm.Colors = colors;
                vm.Categories = categories;
                vm.Tags = tags;

                return View(vm);
            }
            if (!vm.HoverPhoto.CheckFileSize(2, FileSize.Mb))
            {
                ModelState.AddModelError("Photo", "Invalid Photo Size");
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                IEnumerable<Color> colors = await context.Colors.ToListAsync();
                vm.Colors = colors;
                vm.Categories = categories;
                vm.Tags = tags;
                return View(vm);
            }
            StringBuilder sb = new StringBuilder();
            Product product = new Product
            {
                Name = vm.Name,
                Sku = vm.Sku,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                ProductTags = new List<ProductTag>(),
                ProductColors=new List<ProductColor>()

            };
            if (vm.AdditionalPhoto is not null)
            {

                foreach (IFormFile additionalPhoto in vm.AdditionalPhoto)
                {
                    if (!additionalPhoto.CheckFileType(FileType.Image))
                    {
                        sb.AppendLine($"{additionalPhoto.FileName} didnt created!");

                    }
                    if (!additionalPhoto.CheckFileSize(2, FileSize.Mb))
                    {
                        sb.AppendLine($"{additionalPhoto.FileName} didnt created!");

                    }
                    string fileName = await additionalPhoto.CreateFileAsync(env.WebRootPath, "uploads", "images", "products");
                    product.Images.Add(new ProductImage { Type = ImageType.Additional, Url = fileName });
                }
            }
            if(!await context.Categories.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category didnt found!");
                IEnumerable<Category> categories = await context.Categories.ToListAsync();
                IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                vm.Categories = categories;
                vm.Tags = tags;
                return View(vm);
            }
            foreach  (int id  in vm.ColorIds)
            {
                if (!await context.Colors.AnyAsync(c => c.Id == id))
                {
                    ModelState.AddModelError("ColorId", "Color didnt found!");
                    IEnumerable<Category> categories = await context.Categories.ToListAsync();
                    IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                    IEnumerable<Color> colors = await context.Colors.ToListAsync();
                    vm.Categories = categories;
                    vm.Tags = tags;
                    vm.Colors = colors;
                    return View(vm);
                }
                product.ProductColors.Add(new ProductColor { ColorId = id });
            }
            
            foreach (int id in vm.TagIds)
            {

                if (!await context.Tags.AnyAsync(t => t.Id == id))
                {
                    ModelState.AddModelError("TagId", "Tag didnt found!");
                    IEnumerable<Category> categories = await context.Categories.ToListAsync();
                    IEnumerable<Tag> tags = await context.Tags.ToListAsync();
                    vm.Categories = categories;
                    vm.Tags = tags;
                    return View(vm);
                }
                product.ProductTags.Add(new ProductTag { TagId = id });
            }

            
            string mainImg = await vm.MainPhoto.CreateFileAsync(env.WebRootPath,"uploads", "images", "products");
            product.Images.Add(new ProductImage { Type = ImageType.Main, Url = mainImg });
            string hoverImg = await vm.HoverPhoto.CreateFileAsync(env.WebRootPath, "uploads", "images", "products");
            product.Images.Add(new ProductImage { Type = ImageType.Hover, Url = hoverImg });

            await context.AddAsync(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         
        }
        public async Task<IActionResult> Detail(int id)
        {

            if (id <= 0) return BadRequest();
            Product? productFromDb = await context.Products
                .Include(p=>p.Images)
                .Include(p=>p.Category)
                .Include(p=>p.ProductTags).ThenInclude(pt=>pt.Tag)
                .Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (productFromDb == null) return NotFound();
            ProductDetailVM vm = new ProductDetailVM
            {
                Id = productFromDb.Id,
                Name = productFromDb.Name,
                Price = productFromDb.Price,
                Description = productFromDb.Description,
                Sku = productFromDb.Sku,
                CategoryName = productFromDb.Category.Name,
                Tags = productFromDb.ProductTags.Select(pt => pt.Tag).ToList(),
                Colors=productFromDb.ProductColors.Select(pc=>pc.Color).ToList(),
                Images=productFromDb.Images

            };
            return View(vm);

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Product? productFromDb = await context.Products.Include(p=>p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if(productFromDb  == null) return NotFound();
            foreach(ProductImage pi in productFromDb.Images)
            {
                pi.Url.DeleteFile(env.WebRootPath, "uploads", "images", "products");
            }
            context.Products.Remove(productFromDb);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
      
        public async Task<IActionResult> Update (int id)
        {
            if (id <= 0) BadRequest();
            Product? product = await context.Products
                .Include(p=>p.Images)
                .Include(p=>p.ProductTags)
                .Include(p=>p.ProductColors)
                .FirstOrDefaultAsync(p => p.Id == id);
            if(product is null) return NotFound();

            ProductUpdateVM vm = new ProductUpdateVM
            {
                Id=product.Id,
                Name =product.Name,
                Sku=product.Sku,
                Description=product.Description,
                Price=product.Price,
                Categories=await context.Categories.ToListAsync(),
                CategoryId=product.CategoryId,
                Tags=await context.Tags.ToListAsync(),
                TagId=product.ProductTags.Select(pt=>pt.TagId).ToList(),
                Colors=await context.Colors.ToListAsync(),
                ColorId=product.ProductColors.Select(pc=>pc.ColorId).ToList(),
                Images=product.Images
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,ProductUpdateVM vm)
        {
            if (id <= 0) BadRequest();
            Product? product = await context.Products
                .Include(p=>p.Images)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductColors)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Categories = await context.Categories.ToListAsync();
                vm.Tags=await context.Tags.ToListAsync();
                vm.Colors = await context.Colors.ToListAsync();
                vm.Images = product.Images;
                return View(vm);
            }
            if(vm.MainPhoto is not null)
            {
                if(!vm.MainPhoto.CheckFileType(FileType.Image))
                {
                    vm.Categories = await context.Categories.ToListAsync();
                    vm.Images = product.Images;
                    vm.Tags = await context.Tags.ToListAsync();
                    vm.Colors = await context.Colors.ToListAsync();
                    ModelState.AddModelError("MainPhoto", "Please upload photo");
                    return View(vm);
                }
                if (!vm.MainPhoto.CheckFileSize(2, FileSize.Mb))
                {
                    vm.Categories = await context.Categories.ToListAsync();
                    vm.Images = product.Images;
                    vm.Tags = await context.Tags.ToListAsync();
                    vm.Colors = await context.Colors.ToListAsync();
                    ModelState.AddModelError("MainPhoto", "Please upload 2mb than less photo");
                    return View(vm);
                }
            }
            if (vm.HoverPhoto is not null)
            {
                if (!vm.HoverPhoto.CheckFileType(FileType.Image))
                {
                    vm.Categories = await context.Categories.ToListAsync();
                    vm.Images = product.Images;
                    vm.Tags = await context.Tags.ToListAsync();
                    vm.Colors = await context.Colors.ToListAsync();
                    ModelState.AddModelError("HoverPhoto", "Please upload photo");
                    return View(vm);
                }
                if (!vm.HoverPhoto.CheckFileSize(2, FileSize.Mb))
                {
                    vm.Categories = await context.Categories.ToListAsync();
                    vm.Images = product.Images;
                    vm.Tags = await context.Tags.ToListAsync();
                    vm.Colors = await context.Colors.ToListAsync();
                    ModelState.AddModelError("HoverPhoto", "Please upload 2mb than less photo");
                    return View(vm);
                }
            }
           
            if (product.CategoryId!= vm.CategoryId )
            {

                if(!await context.Categories.AnyAsync(c => c.Id == vm.CategoryId))
                {
                    vm.Categories = await context.Categories.ToListAsync();
                    vm.Images = product.Images;
                    vm.Tags = await context.Tags.ToListAsync();
                    vm.Colors = await context.Colors.ToListAsync();
                    ModelState.AddModelError("CategoryId","Category doesnt exists!");
                    return View(vm);
                }
                 product.CategoryId = vm.CategoryId;
            }
            foreach(int tagId in vm.TagId)
            {
                if(!product.ProductTags.Any(pt => pt.Id == tagId))
                {
                    if (!await context.Tags.AnyAsync(t => t.Id == tagId))
                    {
                        vm.Categories = await context.Categories.ToListAsync();
                        vm.Images = product.Images;
                        vm.Tags = await context.Tags.ToListAsync();
                        vm.Colors = await context.Colors.ToListAsync();
                        ModelState.AddModelError("TagId", "TagId doesnt exists!");
                        return View(vm);
                    }
                        product.ProductTags.Add( new ProductTag { TagId=tagId } );
                }
            }
            foreach(ProductTag pt in product.ProductTags)
            {
                if(!vm.TagId.Any(id=>id==pt.TagId))
                {
                    context.ProductTags.Remove(pt);
                }
            }
            foreach (int colorId in vm.ColorId)
            {
                if (!product.ProductColors.Any(pc => pc.Id == colorId))
                {
                    if (!await context.Colors.AnyAsync(c => c.Id == colorId))
                    {
                        vm.Categories = await context.Categories.ToListAsync();
                        vm.Images = product.Images;
                        vm.Tags = await context.Tags.ToListAsync();
                        vm.Colors = await context.Colors.ToListAsync();
                        ModelState.AddModelError("ColorId", "ColorId doesnt exists!");
                        return View(vm);
                    }
                    product.ProductColors.Add(new ProductColor { ColorId = colorId });
                }
            }
            foreach (ProductColor pc in product.ProductColors)
            {
                if (!vm.ColorId.Any(id=> id == pc.ColorId))
                {
                    context.ProductColors.Remove(pc);
                }
            }
            if (vm.MainPhoto is not null)
            {
                string fileName = await vm.MainPhoto.CreateFileAsync(env.WebRootPath, "uploads", "images", "products");
                ProductImage? imageMain =  product.Images.FirstOrDefault(pi => pi.Type == ImageType.Main);
                if (imageMain != null)
                {
                    imageMain.Url.DeleteFile(env.WebRootPath, "uploads", "images", "products");
                    imageMain.Url = fileName;
                }

            }
            if (vm.HoverPhoto is not null)
            {
                string fileName = await vm.HoverPhoto.CreateFileAsync(env.WebRootPath, "uploads", "images", "products");
                ProductImage? imageHover = product.Images.FirstOrDefault(pi => pi.Type == ImageType.Hover);
                if (imageHover != null)
                {
                    imageHover.Url.DeleteFile(env.WebRootPath, "uploads", "images", "products");
                    imageHover.Url = fileName;
                }

            }
            if(vm.ImageId is not null)
            {

                foreach(ProductImage pi in product.Images.Where(pc => pc.Type == ImageType.Additional))
                {
                    if(!vm.ImageId.Any(id=>id==pi.Id))
                    {
                        pi.Url.DeleteFile(env.WebRootPath, "uploads", "images", "products");
                        context.ProductImages.Remove(pi);
                    }
                }
            }
            else product.Images= product.Images.Where(pc=>pc.Type!=ImageType.Additional).ToList();
            TempData["Errors"] = "";
            if(vm.AdditionalPhoto is not null)
            {
                foreach(IFormFile photo in vm.AdditionalPhoto)
                {
                    if (!photo.CheckFileType(FileType.Image))
                    {
                        TempData["Errors"] += $"<p class=\"text-danger\">Please Upload Photo</p>";
                    }
                    else if (!photo.CheckFileSize(2, FileSize.Mb))
                    {
                        TempData["Errors"] += $"<p class=\"text - danger \">Please Upload 2mb than less photo</p>";
                    }
                    else
                    {
                        string fileName = await photo.CreateFileAsync(env.WebRootPath, "uploads", "images", "products");
                        product.Images.Add(new ProductImage { Url = fileName, Type = ImageType.Additional });
                    }

                }
            }
            product.Name=vm.Name;
            product.Sku = vm.Sku;
            product.Price = vm.Price;
            product.Description = vm.Description;

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }




    }
}
