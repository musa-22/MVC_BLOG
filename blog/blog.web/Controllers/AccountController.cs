﻿using blog.web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace blog.web.Controllers
{
    public class AccountController : Controller
    {


        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        //using usermanger & singinmanger which provided from microsfot lib and it will take identity user

        public AccountController(UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager)
        {

            this._userManager = userManager;

            this._signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                var IdentityUser = new IdentityUser

                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email


                };

                var Identitysresult = await _userManager.CreateAsync(IdentityUser, registerViewModel.Password);

                if (Identitysresult.Succeeded)
                {
                    // pass user role

                    var roleIdentityResult = await _userManager.AddToRoleAsync(IdentityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        // if success
                        return RedirectToAction("Register");
                    }

                }
            }

            
            // if error show message
            return View();
        }


        [HttpGet]
        public IActionResult Login(string RetirnUrl )
        {


            var model = new LoginViewModel

            {
                ReturnUrl = RetirnUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {

                var sign = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

                if (sign != null && sign.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");   
        }


        [HttpGet]
        public IActionResult AccessDenied  ()
        {
            return View();
        }
    }
}
