using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.ProductImages
{
    public class ProductImageListItemVM
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
    }
}
