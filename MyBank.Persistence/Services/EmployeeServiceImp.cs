using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;

namespace MyBank.Persistence.Services
{
    public class EmployeeServiceImp : ICustomerService
    {
        private readonly UserManager<Employee> _customerManager;
        private readonly EmployeeContext _customerContext;

        public EmployeeServiceImp(UserManager<Employee> customerManager, EmployeeContext customerContext)
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

        public Boolean CreateTransactionById(TransactionType type, int sourceAccount, int destinationAccount, int transactionTotal, string benificaryName, string message) {

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

        public Boolean CreateTransactionByAccountNumber(TransactionType type, string sourceAccountNumber, string destinationAccountNumber, int transactionTotal, string benificaryName, string message)
        {

            Account? sourceAccount = GetAccountByAccountNumber(sourceAccountNumber);
            Account? destinationAccount = GetAccountByAccountNumber(destinationAccountNumber);
         
            if (sourceAccount == null || destinationAccount == null) {
                return false;
            }

            if (sourceAccount.IsLocked || destinationAccount.IsLocked)
            {
                return false;
            }

            if (sourceAccount.Balance - transactionTotal <= 0) {
                return false;
            }

            Transaction transaction = new Transaction
            {
                Type = type,
                SourceAccountId = sourceAccount.Id,
                DestinationAccountId = destinationAccount.Id,
                TransactionTotal = transactionTotal,
                TransactionExecutionDate = DateTime.Now,
                BenificaryName = benificaryName,
                Message = message
            };

            sourceAccount.Balance -= transactionTotal;
            destinationAccount.Balance += transactionTotal;

            _customerContext.Transactions.Add(transaction);

            _customerContext.SaveChanges();

            return true;
        }

        public IEnumerable<TransactionHistoryDto> GetTransactionsByCustomerName(string customerName) {
            
            int customerId = _customerManager.FindByNameAsync(customerName).Result.Id;

            IEnumerable<int> accountIds = _customerContext.Accounts
                                                .Where(account => account.CustomerId == customerId).ToList()
                                                .Select(account => account.Id);

            IEnumerable<TransactionHistoryDto> transactionHistory = 
                _customerContext.Transactions
                    .Where(transaction => 
                        (accountIds.Contains(transaction.SourceAccountId) || accountIds.Contains(transaction.DestinationAccountId)) 
                        && transaction.TransactionExecutionDate > DateTime.Today.AddMonths(-1)
                    )
                    .Select(transaction => new TransactionHistoryDto { 
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

        public IEnumerable<TransactionHistoryDto> GetTransactionsByCustomerId(int customerId)
        {

            IEnumerable<int> accountIds = _customerContext.Accounts
                                                .Where(account => account.CustomerId == customerId).ToList()
                                                .Select(account => account.Id);

            IEnumerable<TransactionHistoryDto> transactionHistory =
                _customerContext.Transactions
                    .Where(transaction =>
                        (accountIds.Contains(transaction.SourceAccountId) || accountIds.Contains(transaction.DestinationAccountId))
                        && transaction.TransactionExecutionDate > DateTime.Today.AddMonths(-1)
                    )
                    .Select(transaction => new TransactionHistoryDto
                    {
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

            return true;

        }

        public IEnumerable<Customer> FindAllWithAccounts() {
            return _customerContext.Customers.Include("Accounts");
        }

        public void AddMoneyById(int accountId, int amount) {

            //TODO ellenorzed

            Account account = _customerContext.Accounts.Where(account => account.Id == accountId && !account.IsLocked).First();

            if (account != null) {
                Transaction transaction = new Transaction
                {
                    Type = TransactionType.Betet,
                    SourceAccountId = account.Id,
                    DestinationAccountId = account.Id,
                    TransactionTotal = amount,
                    TransactionExecutionDate = DateTime.Now,
                    BenificaryName = "ME",
                    Message = "Betet"
                };

                account.Balance += amount;

                _customerContext.Transactions.Add(transaction);

                _customerContext.SaveChanges();
            }
           
        }

        public void TakeOutMoneyById(int accountId, int amount) {

            //TODO ellenorzed

            Account account = _customerContext.Accounts.Where(account => account.Id == accountId && !account.IsLocked).First();

            if (account != null)
            {
                Transaction transaction = new Transaction
                {
                    Type = TransactionType.Kivet,
                    SourceAccountId = account.Id,
                    DestinationAccountId = account.Id,
                    TransactionTotal = amount,
                    TransactionExecutionDate = DateTime.Now,
                    BenificaryName = "ME",
                    Message = "Kivet"
                };

                if(account.Balance - amount > 0) 
                {
                    account.Balance -= amount;

                    _customerContext.Transactions.Add(transaction);

                    _customerContext.SaveChanges();
                }
             
            }
        }

        public bool LockAccountById(int accountId, bool isLock)
        {
            Account account = _customerContext.Accounts.Where(account => account.Id == accountId).First();

            if (account == null)
            {
                return false;
            }

            account.IsLocked = isLock;

            _customerContext.SaveChanges();

            return true;
        }
    }
}
