using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GA.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
       
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid)
                return View(loginVm);

            var user = await _userManager.FindByNameAsync(loginVm.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "HJKSAHadwadw");
                }
            }

            ModelState.AddModelError("", "User name/password not found");
            return View(loginVm);
        }


        public IActionResult Register()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginVM loginvm)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginvm.UserName };
                var result = await _userManager.CreateAsync(user, loginvm.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginvm);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
