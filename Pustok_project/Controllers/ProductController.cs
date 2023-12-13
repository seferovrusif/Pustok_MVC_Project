using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
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
            int ProductId = id;
            if (id == null || id <= 0) return BadRequest();
            var s = await _context.Product.FindAsync(id);
            //var a = (await _context.ProductImages.Where(x => x.ProductId =ProductId)).;
            if (s == null) return NotFound();
            return View(new UsersProductListItemVM
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
                ///Imagesss=s.ProductImages.Select(a=>a.ProductId).ToList()
            });
          
        }
    }
}
