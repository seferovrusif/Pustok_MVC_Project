using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_project.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string ProfilePicture { get; set; }

    }
}
