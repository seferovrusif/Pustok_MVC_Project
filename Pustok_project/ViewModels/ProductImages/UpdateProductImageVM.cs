using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.ProductImages
{
    public class UpdateProductImageVM
    {
        public IFormFile? ImagePath { get; set; }
        public string? ImagePathStr { get; set; }

        public int ProductId { get; set; }
    }
}
