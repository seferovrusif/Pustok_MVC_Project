using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
using Pustok_project.Helpers;
using Pustok_project.ViewModels.Setting;
using Pustok_project.ViewModels.SliderVM;
using System.Net;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]

    public class SettingController : Controller
    {
        PustokDbContext _db { get; }

        public SettingController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Settings.Select(a => new SettingItemVM
            {
                Id = a.Id,
                Address = a.Address,
                Phone = a.Phone,
                Email = a.Email,
            }).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            var a = await _db.Settings.FindAsync(id);
            if (a == null) return NotFound();
            return View(new UpdateSettingVM
            {
                Address = a.Address,
                Phone = a.Phone,
                Email = a.Email,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateSettingVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.Settings.FindAsync(id);
            if (data == null) return NotFound();

            data.Address = vm.Address;
                data.Phone = vm.Phone;
                data.Email = vm.Email;

            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }

    }
}
