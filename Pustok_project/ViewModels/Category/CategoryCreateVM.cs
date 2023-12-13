using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required, MaxLength(16)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
