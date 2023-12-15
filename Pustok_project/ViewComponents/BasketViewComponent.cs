using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pustok_project.Contexts;
using Pustok_project.ViewModels.BasketVM;

namespace Pustok_project.ViewComponents
{
    public class BasketViewComponent:ViewComponent
    {
        PustokDbContext _context { get; }

        public BasketViewComponent(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var products = _context.Product.Where(p => items.Select(i => i.ProdId).Contains(p.Id));
            List<BasketProductItemVM> basketItems = new();
            foreach (var item in products)
            {
                basketItems.Add(new BasketProductItemVM
                {
                    Id = item.Id,
                    Discount = item.Discount,
                    ImageUrl = item.ProductMainImg,
                    Name = item.Name,
                    Price = item.SellPrice,
                    Count = items.FirstOrDefault(x => x.ProdId == item.Id).Count
                });
            }
            return View(basketItems);
        }
    }
}