using Pustok_project.Contexts;
using Pustok_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.ViewModels.BlogVM;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class BlogController : Controller
    {
        PustokDbContext _db { get; }
        public BlogController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var ms = await _db.Blog.Select(s => new BlogListItemVM
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                AuthorId = s.AuthorId,
                IsDeleted = s.IsDeleted,
                TagId = s.TagBlogs.Select(a => a.TagId).ToList(),

            }).ToListAsync();
            return View(ms);

        }
        public IActionResult Create()
        {
            ViewBag.Author = _db.Author;
            ViewBag.Tag = _db.Tag;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
           
            Blog blogg = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = vm.AuthorId,
                TagBlogs = vm.TagId.Select(id => new TagBlog
                {
                    TagId = id,
                }).ToList(),
                IsDeleted = false

            };
            await _db.Blog.AddAsync(blogg);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Author = _db.Author;
            ViewBag.Tag = _db.Tag;

            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Blog
               .Include(p => p.TagBlogs)
               .SingleOrDefaultAsync(p => p.Id == id);
            
            if (data == null) return NotFound();
            return View(new UpdateBlogVM
            {
                Title = data.Title,
                Description = data.Description,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                AuthorId = data.AuthorId,
                IsDeleted = data.IsDeleted,
                TagId = data.TagBlogs?.Select(i => i.TagId)

            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBlogVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Blog
               .Include(p => p.TagBlogs)
               .SingleOrDefaultAsync(p => p.Id == id);
            //var data = await _db.Blog.FindAsync(id);
            if (vm.TagId != null) { 
                if (!Enumerable.SequenceEqual(data.TagBlogs?.Select(p => p.TagId), vm.TagId))
                {
                    data.TagBlogs = vm.TagId.Select(c => new TagBlog { TagId = c, BlogId = data.Id }).ToList();
                } }
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.AuthorId = vm.AuthorId;
            data.UpdatedAt = DateTime.UtcNow;
            data.IsDeleted = vm.IsDeleted;
            //if (vm.TagId != null)
            //{
            //    data.TagBlogs = vm.TagId.Select(id => new TagBlog
            //    {
            //        TagId = id,
            //    }).ToList();
            //}

            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

            var data = await _db.Blog.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blog.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}





