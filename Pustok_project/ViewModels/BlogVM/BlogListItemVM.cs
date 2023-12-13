using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_project.ViewModels.BlogVM
{
    public class BlogListItemVM
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Title { get; set; }
        [MaxLength(128)]
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AuthorId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<int>? TagId { get; set; }

    }
}
