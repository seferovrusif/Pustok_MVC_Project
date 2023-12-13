using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.Category
{
    public class CategoryListItemVM
    {
        public int Id { get; set; }

        [Required, MaxLength(16)]
        public string Name { get; set; }


    }
}
