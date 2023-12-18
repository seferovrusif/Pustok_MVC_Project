using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Versioning;
using Pustok_project.Contexts;
using Pustok_project.Models;
using Pustok_project.ViewModels.BasketVM;
using Pustok_project.ViewModels.ProductVM;
using System.Collections;

namespace Pustok_project.Controllers
{
    public class ProductController : Controller
    {
        PustokDbContext _context { get; }

        public ProductController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            var s =  await _context.Product.Include(t => t.TagProducts).FirstOrDefaultAsync(p=>p.Id == id);
            if (s == null) return NotFound();
            var a = s.TagProducts.Select(p => p.Tag);
            /*var a = _context.Product.Include(t => t.TagProducts).ThenInclude(tg => tg.Tag);*/

            var vm = new UsersProductListItemVM
            {
                Id = id,
                Name = s.Name,
                About = s.About,
                Description = s.Description,
                SellPrice = s.SellPrice,
                Discount = s.Discount,
                Quantity = s.Quantity,
                ProductMainImg = s.ProductMainImg,
                CategoryId = s.CategoryId,
                ProductCode = s.ProductCode,
                TagProd = s.TagProducts.Select(p => p.Tag)

            };
           
            return View(vm);


        }
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _context.Product.AnyAsync(p => p.Id == id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b => b.ProdId == id);
            if (existItem == null)
            {
                basket.Add(new BasketProductAndCountVM
                {
                    ProdId = (int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return Ok();
        }
    }
}
