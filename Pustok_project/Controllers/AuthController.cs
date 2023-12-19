﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok_project.Models;
using Pustok_project.ViewModels.AuthVM;

namespace Pustok_project.Controllers
{
    public class AuthController : Controller
    {
        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser> _userManager { get; }

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            AppUser user;
            if (!vm.UserNameOrEmail.Contains("@"))
            {
                user= await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            }
            else
            {
                user=await _userManager.FindByNameAsync(vm.UserNameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _userManager.CreateAsync(new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            }, vm.Password);
            //if (!result.Succeeded)
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError("", error.Description);
            //    }
            //    return View(vm);
            //}
            return View();
            return RedirectToAction(nameof(Login));
        }
    }
}

   