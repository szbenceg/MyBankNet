using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;

namespace MyBank.Persistence.Services
{
    public interface ICustomerService
    {
        public IEnumerable<Account> GetAccountsByCustomerName(string customerName);
        public Account GetAccountByAccountNumber(string accountNumber);
        public Boolean CreateTransaction(TransactionType type, int sourceAccount, int destinationAccount, int transactionTotal, string benificaryName, string message);

        public IEnumerable<TransactionHistory> GetTransactionsByCustomerName(string customerName);

        public bool GetIsSecureByUsername(string username);

        public IEnumerable<Customer> FindAllWithAccounts();

    }
}
