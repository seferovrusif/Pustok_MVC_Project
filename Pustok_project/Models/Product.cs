using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_project.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(16)]
        public string ProductCode { get; set; }
        public string ProductHoverImg { get; set; }
        public string ProductMainImg { get; set; }
        [MaxLength(128)]
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName ="smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName ="smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0,100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public IEnumerable<TagProduct>? TagProducts { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
