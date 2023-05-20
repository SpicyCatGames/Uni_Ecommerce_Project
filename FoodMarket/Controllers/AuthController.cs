using FoodMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            var s = HttpContext.Request.Query["returnUrl"];
            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            //if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            //    return Redirect(returnUrl);

            //var user = HttpContext.User;
            //var isAdmin = user.IsInRole("Admin");

            //if(isAdmin)
            //    return RedirectToAction("Index", "Panel");

            var user = await _userManager.FindByNameAsync(vm.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
                return RedirectToAction("Index", "Panel");

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid || vm.Role.Equals("Admin"))
            {
                return View(vm);
            }

            var user = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email
            };

            var role = _roleManager.Roles.FirstOrDefault(role => role.Name.Equals(vm.Role));

            if(role == null)
            {
                return View(vm);
            }

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
