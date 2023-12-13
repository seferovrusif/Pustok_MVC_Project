using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class TagProduct
    {
        public int Id { get; set; }
        [Required]
        public int TagId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Tag? Tag { get; set; }
        public Product? Blog { get; set; }
    }
}
