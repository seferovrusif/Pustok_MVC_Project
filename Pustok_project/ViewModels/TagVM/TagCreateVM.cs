using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.TagVM
{
    public class TagCreateVM
    {
        [MaxLength(64)]
        public string Title { get; set; }

    }
}
