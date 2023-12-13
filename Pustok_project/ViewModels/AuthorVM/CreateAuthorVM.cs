using Pustok_project.Models;
using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.AuthorVM
{
    public class CreateAuthorVM
    {
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
