using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels;
using Pronia.Context;
using Pronia.Extencions;
using Pronia.Extencions.Enums;
using Pronia.Models;
using System.Drawing;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SlideController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public SlideController(AppDbContext context,IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
          IEnumerable<Slide> slides = await context.Slides.ToListAsync();
            return View(slides);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SlideCreateVM slideVM)
        {
            if(!ModelState.IsValid) return View(slideVM);
            if (!slideVM.Photo.ContentType.StartsWith("image/"))
            {
                 ModelState.AddModelError("Photo","Please Upload Image");
                 return View(slideVM);
            }
            if(slideVM.Photo.Length > 2*1024*1024) 
            {
                ModelState.AddModelError("Photo", "Invalid Photo Size");
                return View(slideVM);
            }
            
            string fileName = await slideVM.Photo.CreateFileAsync(env.WebRootPath, "uploads", "images", "slides");
            Slide newSlide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                ButtonText = slideVM.ButtonText,
                Order = slideVM.Order,
                Description = slideVM.Description,
                Image = fileName
            };
            await context.Slides.AddAsync(newSlide);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Slide? slideFromDb= await context.Slides.FirstOrDefaultAsync(s=>s.Id == id);
            if (slideFromDb == null) return NotFound();
            slideFromDb.Image?.DeleteFile(env.WebRootPath, "uploads", "images", "slides");
           
            context.Remove(slideFromDb);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 

        }
        public async Task<IActionResult> Detail(int id)
        {

            if (id <= 0) return BadRequest();
            Slide? slideFromDb = await context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slideFromDb == null) return NotFound();
            return View(slideFromDb);

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide? slide = await context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            SlideUpdateVM vm = new SlideUpdateVM
            {
                Title = slide.Title,
                SubTitle = slide.SubTitle,
                ButtonText=slide.ButtonText,
                Order=slide.Order,
                Description=slide.Description,
                Image=slide.Image

            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,SlideUpdateVM slideVM)
        {
            if (id <= 0) return BadRequest();
            if (!ModelState.IsValid) return View(slideVM);
            Slide? slide = await context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            if(slideVM.Photo is not null)
            {
                if (!slideVM.Photo.CheckFileType(FileType.Image))
                
                {
                    ModelState.AddModelError("Photo", "Please Upload Image");
                    return View(slideVM);
                }
                if (!slideVM.Photo.CheckFileSize(2,FileSize.Mb))
                {
                    ModelState.AddModelError("Photo", "Invalid Photo Size");
                    return View(slideVM);
                }
                slide.Image.DeleteFile(env.WebRootPath, "uploads", "images", "slides");
                string fileName= await  slideVM.Photo.CreateFileAsync(env.WebRootPath, "uploads", "images", "slides");
                slide.Image = fileName;
               
            }
            slide.Title = slideVM.Title;
            slide.SubTitle=slideVM.SubTitle;
            slide.Order = slideVM.Order;
            slide.Description = slideVM.Description;
            await context.SaveChangesAsync();

           
            return RedirectToAction(nameof(Index));
        }
    }
}
