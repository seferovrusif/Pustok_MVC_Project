using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
using Pustok_project.Models;
using Pustok_project.ViewModels.AuthorVM;
using Pustok_project.ViewModels.Category;
using Pustok_project.ViewModels.Common;
using Pustok_project.ViewModels.TagVM;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        PustokDbContext _db { get; }

        public TagController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = _db.Tag.Take(take).Select(p => new TagListItemVM
            {
                Id = p.Id,
                Title = p.Title
            });
            int count = await _db.Tag.CountAsync();
            PaginationVM<IEnumerable<TagListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / take), items);
            //var ms = await _db.Tag.Select(t => new TagListItemVM
            //{
            //    Id = t.Id,
            //    Title = t.Title
            //}).ToListAsync();
            return View(pag);

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
                Title = vm.Title,
            };
            await _db.Tag.AddAsync(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Tag.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateTagVM
            {
                Title = data.Title
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTagVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.Tag.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;

            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

            var data = await _db.Tag.FindAsync(id);
            if (data == null) return NotFound();
            _db.Tag.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> TagLoadMore(int page = 1, int count = 8)
        {
            var items = _db.Tag.Skip((page - 1) * count).Take(count).Select(p => new TagListItemVM
            {
                Id = p.Id,
               Title=p.Title
            });
            int totalCount = await _db.Tag.CountAsync();
            PaginationVM<IEnumerable<TagListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);

            return PartialView("_TagPaginationPartial", pag);
        }

    }
}
