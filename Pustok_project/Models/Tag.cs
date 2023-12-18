using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        public ICollection<TagBlog>? TagBlogs { get; set; }
        public IEnumerable<TagProduct>? TagProducts { get; set; }

    }
}
