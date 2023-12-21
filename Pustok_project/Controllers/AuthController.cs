using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok_project.Helpers;
using Pustok_project.Models;
using Pustok_project.ViewModels.AuthVM;
using System.Security.Claims;

namespace Pustok_project.Controllers
{
    public class AuthController : Controller
    {
        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser> _userManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }


        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, LoginVM vm)
        {
            AppUser user;
            if (vm.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                return View(vm);
            }
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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
            var user = new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong. Please contact admin");
                return View(vm);
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        [Authorize(Roles = "SuperAdmin, Admin, Moderator,Member")]

        public async  Task<IActionResult> UserPage()
        {
			
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			if (user == null)
			{
				ModelState.AddModelError("", "User is not found");
				return RedirectToAction("Index", "Home");
			}
			return View(new UserPageVM
			{
				Fullname = user.Fullname,
				Email = user.Email,
				Username = user.UserName,

			});
		}
        [HttpPost]
        public async Task<IActionResult> UserPage(UserPageVM vm)
        {
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null ) return NotFound();
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index", "Home");
			}

			user.UserName = vm.Username;
			user.Email = vm.Email;
            user.Fullname = vm.Fullname;
            await _userManager.UpdateAsync(user);

            if (User.Identity.Name != vm.Username)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("UserPage", "Auth");

            }
            //if(User.Identity.Name != vm.Username)
            //{
            //    await _signInManager.SignOutAsync();
            //    await _signInManager.CheckPasswordSignInAsync(user, vm.Password,true);////pasword hash problem olur sonraya saxladim bunu
            //}
            return View(); 



            ////ozum ucun test eledim alinmadi :/
            ///
            //var user1 = User as ClaimsPrincipal;

            //var oldUsernameClaim = user1.FindFirst(ClaimTypes.Name);
            //var identity = user1.Identity as ClaimsIdentity;
            //if (oldUsernameClaim != null)
            //{
            //    identity.RemoveClaim(oldUsernameClaim);
            //}

            //// Add the new username claim
            //var newUsernameClaim = new Claim(ClaimTypes.Name, vm.Username);
            //var newIdentity = new ClaimsIdentity(identity.Claims.Append(newUsernameClaim), identity.AuthenticationType);
            //var newPrincipal = new ClaimsPrincipal(newIdentity);

            //// Update the user's identity
            //HttpContext.SignInAsync(newPrincipal);


        }
       

    }
}

