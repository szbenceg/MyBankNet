using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Services;
using MyBank.ViewModel;

namespace MyBank.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public TransactionController(ICustomerService customerService, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _customerService = customerService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Transaction(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult TransactionHistory()
        {
            ViewBag.IsSecure = _customerService.GetIsSecureByUsername(User.Identity.Name);
            ViewBag.CustomerAccountNumber = _customerService.GetAccountsByCustomerName(User.Identity.Name).Select(account => account.AccountNumber);
            ViewBag.TransactionList = _customerService.GetTransactionsByCustomerName(User.Identity.Name);

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransactionHistory(TransactionViewModelHistory vmHistory)
        {
            if (_customerService.GetIsSecureByUsername(User.Identity.Name))
            {
                var user = await _userManager.FindByNameAsync(vmHistory.UserName == null ? "" : vmHistory.UserName);
                if (user == null || vmHistory.Password == null)
                {
                    ViewBag.IsSecure = true;
                    ModelState.AddModelError("", "Helytelen felhasználói adatok!");
                    return View("TransactionHistory", vmHistory);
                }
                var result = await _signInManager.PasswordSignInAsync(user, vmHistory.Password, false, false);

                if (!result.Succeeded)
                {
                    ViewBag.IsSecure = true;
                    ModelState.AddModelError("", "Helytelen felhasználói adatok!");
                    return View("TransactionHistory", vmHistory);
                }
                ViewBag.TransactionList = _customerService.GetTransactionsByCustomerName(User.Identity.Name);
                ViewBag.CustomerAccountNumber = _customerService.GetAccountsByCustomerName(User.Identity.Name).Select(account => account.AccountNumber);
            }

            ViewBag.IsSecure = false;
            return View("TransactionHistory", vmHistory);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TransactionSuccess(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transaction(TransactionViewModel transaction)
        {
            if (!ModelState.IsValid)
                return View("Transaction", transaction);

            if (_customerService.GetIsSecureByUsername(User.Identity.Name)) {
                var user = await _userManager.FindByNameAsync(transaction.UserName == null ? "" : transaction.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Helytelen felhasználói adatok!");
                    return View("Transaction", transaction);
                }
                var result = await _signInManager.PasswordSignInAsync(user, transaction.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Helytelen felhasználói adatok!");
                    return View("Transaction", transaction);
                }
            }

            Account sourceAccount = _customerService.GetAccountByAccountNumber(transaction.SourceAccount);

            if (sourceAccount.Balance - transaction.Amount < 0) {
                ModelState.AddModelError("", "Az egyenlegen nincs elegendő összeg az átutaláshoz");
                return View("Transaction", transaction);
            }

            if (sourceAccount.IsLocked)
            {
                ModelState.AddModelError("", "A számla zárolva lett, kérjük keresse fel egyik fiókunkat.");
                return View("Transaction", transaction);
            }

            Account destinationAccount = _customerService.GetAccountByAccountNumber(transaction.DestinationAccountNumber);

            if (destinationAccount != null)
            {
                destinationAccount.Balance += transaction.Amount;
                sourceAccount.Balance -= transaction.Amount;
                _customerService.CreateTransactionById(TransactionType.Transfer, sourceAccount.Id, destinationAccount.Id, transaction.Amount, transaction.BenificaryName, transaction.Message);
            }
            else {
                ModelState.AddModelError("", "A számla nem található");
                return View("Transaction", transaction);
            }

            return View("TransactionSuccess", transaction);
        }
    }
}
