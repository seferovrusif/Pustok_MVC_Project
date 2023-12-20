﻿using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.AuthVM
{
    public class UserPageVM
    {
        [Required(ErrorMessage = "Ad və soy adınızı daxil edin!!!"), MaxLength(36)]
        public string Fullname { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "İstifadəçi adınızı daxil edin!!!"), MaxLength(24)]
        public string Username { get; set; }
        [ DataType(DataType.Password), Compare(nameof(ConfirmPassword))]
        //RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")
        public string? Password { get; set; }
        [ DataType(DataType.Password)]
        //RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")
        public string? ConfirmPassword { get; set; }
        //public string CurrentPassword { get; set; }
	}
}