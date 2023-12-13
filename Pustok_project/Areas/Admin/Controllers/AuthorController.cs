using Pustok_project.Contexts;
using Pustok_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.ViewModels.AuthorVM;
using System.Xml.Linq;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        PustokDbContext _db { get; }
        public AuthorController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var ms = await _db.Author.Select(s => new AuthorListItemVM
            {
                Id = s.Id,
                Name=s.Name,
                Surname=s.Surname,
                IsDeleted=s.IsDeleted
            }).ToListAsync();
            return View(ms);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorVM vm)
        {
         
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Author Author = new Author
            {
                Name = vm.Name,
                Surname = vm.Surname,
                IsDeleted = vm.IsDeleted
            };
            await _db.Author.AddAsync(Author);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateAuthorVM
            {
                Name = data.Name,
                Surname = data.Surname,
                IsDeleted = data.IsDeleted
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update (int id , UpdateAuthorVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
 
            var data = await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
                data.Name = vm.Name;
                data.Surname = vm.Surname;
                data.IsDeleted = vm.IsDeleted;


            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

             var data =await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
            _db.Author.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
    }





