using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBank.Controllers;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Services;
using MyBank.ViewModel;

namespace MyBank.Views.Account
{
    public class AccountController : BaseController
    {
        private readonly UserManager<Customer> _userManager;
        private readonly UserManager<Employee> _employeeManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ICustomerService _customerService;

        public AccountController(UserManager<Customer> userManager,
                                 SignInManager<Customer> signInManager,
                                 UserManager<Employee> employeeManager,
                                 ICustomerService customerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
            _employeeManager = employeeManager;
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
                user.IsSecure = userVm.IsSecure;
                await _userManager.UpdateAsync(user);
                if (user == null || user.PinCode != userVm.PinCode)
                {
                    ModelState.AddModelError("", "Sikertelen bejelentkezés!");
                    return View(userVm);
                }

                if (!_customerService.GetAccountsByCustomerName(userVm.UserName).Select(account => account.AccountNumber).Contains(userVm.AccountNumber)) {
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


            var customers = new Employee
            {
                Name = user.Name,
                //PinCode = user.PinCode,
                UserName = user.UserName,
                //Accounts = new List<MyBank.Persistence.Dao.Account> {
                //    new MyBank.Persistence.Dao.Account {
                //        AccountNumber = user.AccountNumber,
                //        Balance = 1000000,
                //        Created = DateTime.Now,
                //    }
                //},
            };

            var result = await _employeeManager.CreateAsync(customers, user.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Register", user);
            }

            //await _signInManager.SignInAsync(customers, false); 
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
