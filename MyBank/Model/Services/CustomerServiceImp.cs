using Microsoft.AspNetCore.Identity;
using MyBank.Model.Dao;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyBank.Model.Services
{
    public class CustomerServiceImp : ICustomerService
    {
        private readonly UserManager<Customer> _customerManager;
        private readonly CustomerContext _customerContext;

        public CustomerServiceImp(UserManager<Customer> customerManager, CustomerContext customerContext)
        {
            _customerManager = customerManager;
            _customerContext = customerContext;
        }


        public IEnumerable<Account>? GetAccountsAsync(string customerName)
        {
            int customerId = _customerManager.FindByNameAsync(customerName).Result.Id;

            var result = _customerContext.Accounts.Where(account => account.CustomerId == customerId);

            return result.Any() ? result.ToList() : null;
        }
    }
}
