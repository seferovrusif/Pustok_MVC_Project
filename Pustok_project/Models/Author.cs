using System.ComponentModel.DataAnnotations;

namespace Pustok_project.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
