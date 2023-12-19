using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
using Pustok_project.Models;
using Pustok_project.ViewModels.ProductImages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok_project.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]

    public class ProductImagesController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductImagesController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            var ms = await _db.ProductImages.Select(s => new ProductImageListItemVM
            {
               Id = s.Id,   
               ImagePath = s.ImagePath,
               ProductId = s.ProductId
            }).ToListAsync();
            return View(ms);

        }
        public IActionResult Create()
        {
            ViewBag.Product = _db.Product;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductImageVM vm)
        {
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            ProductImage prodimg = new ProductImage
            {
                ImagePath = await vm.ImageFile.SaveAsync(PathConstants.Productimages),
                ProductId = vm.ProductId

            };
            await _db.ProductImages.AddAsync(prodimg);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Product = _db.Product;

            if (id == null || id <= 0) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateProductImageVM
            {
               ImagePathStr = data.ImagePath,
               ProductId = data.ProductId
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductImageVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
                data.ProductId = vm.ProductId;
            if (vm.ImagePath != null)
            {
                data.ImagePath =await vm.ImagePath.SaveAsync(PathConstants.Productimages);
            }
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImages.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}

