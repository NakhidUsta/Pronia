using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Manage.ViewModels;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Extencions;
using Pronia.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels.Tags;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public TagController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> tags = await context.Tags.ToListAsync();
            return View(tags);
        }
      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
               
                return View(vm);
            }
          
           
            Tag tag = new Tag
            {
                Name = vm.Name,
               

            };
           

            await context.AddAsync(tag);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Detail(int id)
        {

            if (id <= 0) return BadRequest();
            Tag? tagFromDb = await context.Tags.FirstOrDefaultAsync(t=>t.Id== id);
                
            if (tagFromDb == null) return NotFound();
            
            return View(tagFromDb);

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Tag? tagFromDb = await context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tagFromDb == null) return NotFound();
           
            context.Tags.Remove(tagFromDb);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Tag tag)
        {
            if (id <= 0) return BadRequest();
            Tag? tagFromDb = await context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tagFromDb is null) return NotFound();
            if ((await context.Tags.AnyAsync(t => t.Name == tag.Name)))
            {
                ModelState.AddModelError("Name", "Tag with this name already exists!");
                return View();
            }
            tagFromDb.Name = tag.Name;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
