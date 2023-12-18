using Pustok_project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pustok_project.ViewModels.ProductVM
{
    public class UsersProductListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        [MaxLength(128)]
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        public string ProductMainImg { get; set; }
        public int CategoryId { get; set; }
        public ICollection<int>? TagId { get; set; }
        public IEnumerable<Tag>? Tag { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public IEnumerable<string>? Imagesss { get; set; }
 


    }
}
