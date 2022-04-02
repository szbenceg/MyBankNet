using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBank.Controllers;
using MyBank.Model.Dao;
using MyBank.ViewModel;

namespace MyBank.Views.Account
{
    public class AccountController : BaseController
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public AccountController(UserManager<Customer> userManager,
                                 SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel userVm, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userVm.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Sikertelen bejelentkezés!");
                    return View(userVm);
                }

                var result = await _signInManager.PasswordSignInAsync(user, userVm.Password, false, false);


                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Sikertelen bejelentkezés!");
            }

            return View("Login", userVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Register", user);


            var customers = new Customer
            {
                Name = user.UserName,
                UserName = user.UserName,
                Accounts = new List<MyBank.Model.Dao.Account> {
                    new MyBank.Model.Dao.Account {
                        AccountNumber = user.AccountNumber,
                        Balance = 1000000,
                        Created = DateTime.Now,
                    }
                },
            };

            var result = await _userManager.CreateAsync(customers, user.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Register", user);
            }

            await _signInManager.SignInAsync(customers, false); 
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
