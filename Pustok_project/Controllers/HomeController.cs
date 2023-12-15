using Microsoft.AspNetCore.Mvc;
using Pustok_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Pustok_project.Contexts;
using Pustok_project.ViewModels.HomeVM;
using Pustok_project.ViewModels.SliderVM;
using Pustok_project.ViewModels.ProductVM;
using Humanizer;

namespace Pustok_project.Controllers
{
    public class HomeController : Controller
    {
        PustokDbContext _context { get; }

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM
            {
                Sliders = await _context.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                    IsLeft = s.IsLeft,
                    Title = s.Title,
                    Text = s.Text,
                }).ToListAsync(),


            Products = await _context.Product.Where(p => !p.IsDeleted).Select(p => new UsersProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                About = p.About,
                Description = p.Description,
                SellPrice = p.SellPrice,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                ProductMainImg = p.ProductMainImg
            }).ToListAsync()
            };
            return View(vm);
        }
        public string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key] ?? "";
        }
        public IActionResult GetBasket()
        {
            return ViewComponent("Basket");
        }

    }
}
