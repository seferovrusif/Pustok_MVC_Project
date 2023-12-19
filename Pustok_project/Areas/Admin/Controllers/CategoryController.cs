using Microsoft.AspNetCore.Mvc;
using Pustok_project.ViewModels.Category;
using Pustok_project.Contexts;
using Microsoft.EntityFrameworkCore;
using Pustok_project.ViewModels.SliderVM;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class CategoryController : Controller
    {
        PustokDbContext _db { get; }
        public CategoryController(PustokDbContext db)
        {
            _db = db;
        }



        public async Task<IActionResult> Index()
        {
            var ms = await _db.Category.Select(s => new ViewModels.Category.CategoryListItemVM
            {
                Name = s.Name,
                Id = s.Id,
            })
            .Take(2)
                .ToListAsync();
            return View(ms);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Category.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Category.AddAsync(new Models.Category { Name = vm.Name });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Category.FindAsync(id);
            if (data == null) return NotFound();
            return View(new CategoryUpdateVM
            {
                Name = data.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.Category.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;


            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

            var data = await _db.Category.FindAsync(id);
            if (data == null) return NotFound();
            _db.Category.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ShowMoreButton(int page = 1, int pageSize = 2)
        {
            var records = _db.Category.ToList()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(records);
        }
        public IActionResult GetMoreRecords(int page = 1, int pageSize = 5)
        {
            var records = _db.Category.Select(c => new CategoryListItemVM
            {
                Id = c.Id,
                Name = c.Name
            })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_RecordPartial", records);
        }
    }
}
