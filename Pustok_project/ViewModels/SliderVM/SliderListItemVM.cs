using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.SliderVM
{
    public class SliderListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Text { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }
        public bool IsLeft { get; set; }


    }
}
