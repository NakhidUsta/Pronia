﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels;
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
        public async Task<IActionResult>Index(int page =1, int take =5)
        {
            IEnumerable<Category> categories = await context.Categories
                .Skip((page-1)*take)
                .Take(take)
                .Include(c => c.Products).ToListAsync();
            int categoryCount=await context.Categories.CountAsync();
            return View(new CategoryIndexVm
            {
                Categories=categories,
                CurrentPage=page,
                TotalPage=(int)Math.Ceiling((decimal) categoryCount/take)
            });
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid) { return View(category); }
            if((await context.Categories.AnyAsync(c => c.Name == category.Name)))
            {
                ModelState.AddModelError("Name", "Category with this name already exists!");
                return View();
            }
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
             Category? category= await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Category category)
        {
            if (id <= 0) return BadRequest();
          Category? categoryFromDb= await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(categoryFromDb is null) return NotFound();
            if ((await context.Categories.AnyAsync(c => c.Name == category.Name)))
            {
                ModelState.AddModelError("Name", "Category with this name already exists!");
                return View();
            }
           categoryFromDb.Name = category.Name ;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          
           
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Category? categoryFromDb = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            context.Categories.Remove(categoryFromDb);
             await   context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            Category? categoryFromDb = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (categoryFromDb == null) return NotFound();
            return View(categoryFromDb);

        }

    }
}
