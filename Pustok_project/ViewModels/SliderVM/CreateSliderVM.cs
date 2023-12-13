using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.SliderVM
{
    public class CreateSliderVM
    {
        [Required, MinLength(3), MaxLength(64)]
        public string Title { get; set; }

        [Required, MinLength(3), MaxLength(256)]
        public string Text { get; set; }

        [Required, Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required]
        public IFormFile ImageUrl { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
