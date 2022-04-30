using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;

namespace MyBank.Persistence.Services
{
    public interface ICustomerService
    {
        public IEnumerable<Account> GetAccountsByCustomerName(string customerName);
        public Account GetAccountByAccountNumber(string accountNumber);
        public Boolean CreateTransactionById(TransactionType type, int sourceAccount, int destinationAccount, int transactionTotal, string benificaryName, string message);

        public IEnumerable<TransactionHistoryDto> GetTransactionsByCustomerName(string customerName);

        public bool GetIsSecureByUsername(string username);

        public IEnumerable<Customer> FindAllWithAccounts();

        public IEnumerable<TransactionHistoryDto> GetTransactionsByCustomerId(int customerId);

        public void AddMoneyById(int accountId, int amount);

        public void TakeOutMoneyById(int accountId, int amount);

        public Boolean CreateTransactionByAccountNumber(TransactionType type, string sourceAccountNumber, string destinationAccountNumber, int transactionTotal, string benificaryName, string message);

        public Boolean LockAccountById(int accountId, bool isLock);

    }
}
