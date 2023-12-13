using Pustok_project.Contexts;
using Pustok_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.ViewModels.SliderVM;
using Pustok_project.Helpers;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env;
        public SliderController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var ms = await _db.Sliders.Select(s => new SliderListItemVM
            {
                Title = s.Title,
                Text = s.Text,
                ImageUrl = s.ImageUrl,
                Price= s.Price,
                Id = s.Id,
            }).ToListAsync();
            return View(ms);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM vm)
        {
         
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {

                Title = vm.Title,
                Text = vm.Text,
                ImageUrl = await vm.ImageUrl.SaveAsync(PathConstants.Productimages),
                Price = vm.Price,
                IsLeft = false

            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateSliderVM
            {
                ImageUrlstr = data.ImageUrl,
                Text = data.Text,
                Title = data.Title,
                Price = data.Price,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update (int id , UpdateSliderVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
 
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Text = vm.Text;
            data.Title = vm.Title;
            if (vm.ImageUrl != null)
            {
                data.ImageUrl = await vm.ImageUrl.SaveAsync(PathConstants.Productimages);
            }
            data.Price = vm.Price;
         
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

             var data =await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
    }





