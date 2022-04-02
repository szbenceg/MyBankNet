using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.Model.Dao;
using MyBank.Model.Services;
using MyBank.ViewModel;

namespace MyBank.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ICustomerService _customerService;

        public TransactionController(ICustomerService customerService)
        {
            _customerService = customerService;
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
        public IActionResult TransactionHistory(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
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

            Account sourceAccount = _customerService.GetAccountByAccountNumber(transaction.SourceAccount);

            if (sourceAccount.Balance - transaction.Amount < 0) {
                ModelState.AddModelError("", "Az egyenlegen nincs elegendő összeg az átutaláshoz");
                return View("Transaction", transaction);
            }

            Account destinationAccount = _customerService.GetAccountByAccountNumber(transaction.DestinationAccountNumber);

            if (destinationAccount != null)
            {
                destinationAccount.Balance += transaction.Amount;
                sourceAccount.Balance -= transaction.Amount;
                _customerService.CreateTransaction(TransactionType.Transfer, sourceAccount.Id, destinationAccount.Id, transaction.Amount, transaction.BenificaryName, transaction.Message);
            }
            else {
                ModelState.AddModelError("", "A számla nem található");
                return View("Transaction", transaction);
            }

            return View("TransactionSuccess", transaction);
        }
    }
}
