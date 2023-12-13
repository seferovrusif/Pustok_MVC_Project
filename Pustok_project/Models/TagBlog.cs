using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class TagBlog
    {
        public int Id { get; set; }
        [Required]
        public int TagId { get; set; }
        [Required]
        public int BlogId { get; set; }
        public Tag? Tag { get; set; }
        public Blog? Blog { get; set; }

    }
}
