using Pustok_project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_project.ViewModels.ProductVM
{
    public class CreateProductVM
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(16)]
        public string ProductCode { get; set; }
        [MaxLength(128)]
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public int CategoryId { get; set; }

        public IFormFile ProductMainImg { get; set; }
        public IFormFile ProductHoverImg { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection <int>? TagId { get; set; }

        public IEnumerable<IFormFile>? ProductImages { get; set; }

    }
}
