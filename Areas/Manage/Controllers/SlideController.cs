using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SlideController : Controller
    {
        private readonly AppDbContext context;

        public SlideController(AppDbContext context)
        {
            this.context = context;
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
        public async Task<IActionResult> Create(Slide slide)
        {
            if(!ModelState.IsValid) return View();
            if (!slide.Photo.ContentType.StartsWith("image/"))
            {
                 ModelState.AddModelError("Photo","Please Upload Image");

            }
            if(slide.Photo.Length > 2*1024*1024) 
            {
                ModelState.AddModelError("Photo", "Invalid Photo Size");
            }
            using (FileStream fs = new FileStream("C:\\Users\\Huawei\\OneDrive\\Desktop\\BACKEND\\C#\\Pronia\\Pronia\\wwwroot\\assets\\images\\website-images\\"+slide.Photo.FileName,FileMode.Create))
            {
                await slide.Photo.CopyToAsync(fs);
            }
            slide.Image = slide.Photo.FileName;
            await context.Slides.AddAsync(slide);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Slide? slideFromDb= await context.Slides.FirstOrDefaultAsync(s=>s.Id == id);
            if (slideFromDb == null) return NotFound();
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
    }
}
