using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class ProductImage
    {
        public int  Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
