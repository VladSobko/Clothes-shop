using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shop_proj.Models;
using shop_proj.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userman;
        private readonly RoleManager<User> _roleManager;
        private readonly SignInManager<User> signman;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            userman = userManager;
            signman = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register reg)
        {
            if (ModelState.IsValid)
            {          
               
                User user = new User { Email = reg.Email, UserName = reg.Email, Year = reg.Year };
               
                var result = await userman.CreateAsync(user, reg.Password);
                if (result.Succeeded)
                {
                    List<string> rol1 = new List<string>();
                    rol1.Add("user");
                    await signman.SignInAsync(user, false);
                    await userman.AddToRolesAsync(user, rol1);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(reg);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new Login { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login obj)
        {
            if (ModelState.IsValid)
            {
                var result = await signman.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(obj.ReturnUrl) && Url.IsLocalUrl(obj.ReturnUrl))
                    {
                        return Redirect(obj.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін чи пароль");
                }
            }
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await signman.SignOutAsync();
            return RedirectToAction("Index", "User");
        }
    }

}
