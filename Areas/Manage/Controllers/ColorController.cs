using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Manage.ViewModels;
using Pronia.Context;
using Pronia.Extencions.Enums;
using Pronia.Extencions;
using Pronia.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ColorController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ColorController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Color> colors = await context.Colors.ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ColorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {

                return View(vm);
            }


            Color color = new Color
            {
                Name = vm.Name,


            };


            await context.AddAsync(color);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Detail(int id)
        {

            if (id <= 0) return BadRequest();
            Color? colorFromDb = await context.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (colorFromDb == null) return NotFound();

            return View(colorFromDb);

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Color? colorFromDb = await context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (colorFromDb == null) return NotFound();

            context.Colors.Remove(colorFromDb);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Color? colorFromDb = await context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (colorFromDb == null)
            {
                return NotFound();
            }
            return View(colorFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Color color)
        {
            if (id <= 0) return BadRequest();
            Color? colorFromDb = await context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (colorFromDb is null) return NotFound();
            if ((await context.Colors.AnyAsync(c => c.Name == color.Name)))
            {
                ModelState.AddModelError("Name", "Tag with this name already exists!");
                return View();
            }
            colorFromDb.Name = color.Name;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
