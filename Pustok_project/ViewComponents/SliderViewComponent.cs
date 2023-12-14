using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
using Pustok_project.ViewModels.SliderVM;

namespace Pustok_project.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        PustokDbContext _db { get; }

        public SliderViewComponent(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _db.Sliders.Select(s => new SliderListItemVM
            {
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                Price=s.Price,
                IsLeft = s.IsLeft,
                Title = s.Title,
                Text = s.Text,
            }).ToListAsync());
        }
    }
}
