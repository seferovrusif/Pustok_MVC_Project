using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.AuthVM
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
