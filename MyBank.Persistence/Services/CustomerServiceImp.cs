using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;

namespace MyBank.Persistence.Services
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

        public IEnumerable<Account>? GetAccountsByCustomerName(string customerName)
        {
            int customerId = _customerManager.FindByNameAsync(customerName).Result.Id;

            var result = _customerContext.Accounts.Where(account => account.CustomerId == customerId);

            return result.Any() ? result.ToList() : null;
        }

        public Account? GetAccountByAccountNumber(string accountNumber)
        {
            Account? result = _customerContext.Accounts.Where(account => account.AccountNumber == accountNumber).FirstOrDefault();

            return result;
        }

        public Boolean CreateTransaction(TransactionType type, int sourceAccount, int destinationAccount, int transactionTotal, string benificaryName, string message) {

            Transaction transaction = new Transaction {
                Type = type,
                SourceAccountId = sourceAccount,
                DestinationAccountId = destinationAccount,
                TransactionTotal = transactionTotal,
                TransactionExecutionDate = DateTime.Now,
                BenificaryName = benificaryName,
                Message = message
            };

            _customerContext.Transactions.Add(transaction);

            _customerContext.SaveChanges();

            return true;
        }

        public IEnumerable<TransactionHistory> GetTransactionsByCustomerName(string customerName) {
            
            int customerId = _customerManager.FindByNameAsync(customerName).Result.Id;

            IEnumerable<int> accountIds = _customerContext.Accounts
                                                .Where(account => account.CustomerId == customerId).ToList()
                                                .Select(account => account.Id);

            IEnumerable<TransactionHistory> transactionHistory = 
                _customerContext.Transactions
                    .Where(transaction => 
                        (accountIds.Contains(transaction.SourceAccountId) || accountIds.Contains(transaction.DestinationAccountId)) 
                        && transaction.TransactionExecutionDate > DateTime.Today.AddMonths(-1)
                    )
                    .Select(transaction => new TransactionHistory { 
                        TransactionTotal = transaction.TransactionTotal,
                        ExecutionDate = transaction.TransactionExecutionDate,
                        BenificaryName = transaction.BenificaryName,
                        TransactionType = transaction.Type.ToString(),
                        SourceAccountNumber = _customerContext.Accounts.Where(account => account.Id == transaction.SourceAccountId).FirstOrDefault().AccountNumber,
                        DestinationAccountNumber = _customerContext.Accounts.Where(account => account.Id == transaction.DestinationAccountId).FirstOrDefault().AccountNumber,
                        Message = transaction.Message
                    })
                    .OrderByDescending(transaction => transaction.ExecutionDate);

            return transactionHistory;

        }

        public bool GetIsSecureByUsername(string customerName) {

            bool isSecure = _customerManager.FindByNameAsync(customerName).Result.IsSecure;

            return isSecure;

        }

        public IEnumerable<Customer> FindAllWithAccounts() {
            return _customerContext.Customers.Include("Accounts");
        }

    }
}
