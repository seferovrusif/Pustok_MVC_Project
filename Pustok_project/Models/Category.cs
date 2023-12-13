using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? ParentCategoryId { get; set; }

    }
}
