using Microsoft.AspNetCore.Mvc;
using Pustok_project.Contexts;
using Pustok_project.ViewModels.ProductVM;
using Microsoft.EntityFrameworkCore;

namespace Pustok_project.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        PustokDbContext _context { get; }

        public ProductViewComponent(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Product.Where(a=>a.CategoryId==1).Select(p => new UsersProductListItemVM
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
            }).ToListAsync());
        }

      
    }
}
