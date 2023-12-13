using Pustok_project.Models;
using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.ProductImages
{
    public class CreateProductImageVM
    {
        public IFormFile ImageFile { get; set; }
        public int ProductId { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
