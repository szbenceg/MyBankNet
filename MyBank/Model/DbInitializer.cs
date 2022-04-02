using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBank.Model.Dao;
using MyBank.Model.Services;

namespace MyBank.Model
{
    public class DbInitializer
    {
        private static CustomerContext _customerContext = null!;
        private static UserManager<Customer> _userManager = null!;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _customerContext = serviceProvider.GetRequiredService<CustomerContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();

            _customerContext.Database.Migrate();

            if (_customerContext.Customers.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

        }

        private static void CreateTestCustomer()
        {

            var customers = new Customer[]
            {
                new Customer {
                    Name = "Test Elek",
                    UserName = "TestElek",
                    Accounts = new List<Account> { 
                        new Account {
                            AccountNumber = "1111",
                            Balance = 1000000,
                        } 
                    },
                },
            };


            foreach (Customer customer in customers)
            {
                var result = _userManager.CreateAsync(customer, "Password1");
            }
        }
    }
}
