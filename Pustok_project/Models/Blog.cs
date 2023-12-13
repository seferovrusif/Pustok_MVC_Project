using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<TagBlog>? TagBlogs { get; set; }
    }
}
