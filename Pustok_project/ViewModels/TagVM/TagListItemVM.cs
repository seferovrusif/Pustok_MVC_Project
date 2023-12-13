using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.TagVM
{
    public class TagListItemVM
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }

    }
}
