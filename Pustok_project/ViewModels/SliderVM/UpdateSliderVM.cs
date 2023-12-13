using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.SliderVM
{
    public class UpdateSliderVM
    {
        [Required, MinLength(3), MaxLength(64)]
        public string Title { get; set; }

        [Required, MinLength(3), MaxLength(256)]
        public string Text { get; set; }

        [Required, Range(0, float.MaxValue)]
        public float Price { get; set; }

        public string? ImageUrlstr { get; set; }
        public IFormFile? ImageUrl { get; set; }


    }
}
