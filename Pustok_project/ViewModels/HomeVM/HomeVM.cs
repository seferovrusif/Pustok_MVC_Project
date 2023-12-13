using Pustok_project.ViewModels.SliderVM;
using Pustok_project.ViewModels.ProductVM;

namespace Pustok_project.ViewModels.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<SliderListItemVM>? Sliders { get; set; }
        public IEnumerable<UsersProductListItemVM>? Products { get; set; }
    }
}
