using MyBank.Model.Dao;
using MyBank.ViewModel;

namespace MyBank.Model.Services
{
    public interface ICustomerService
    {
        public IEnumerable<Account> GetAccountsByCustomerName(string customerName);
        public Account GetAccountByAccountNumber(string accountNumber);
        public Boolean CreateTransaction(TransactionType type, int sourceAccount, int destinationAccount, int transactionTotal, string benificaryName, string message);

        public IEnumerable<TransactionViewModelHistory> GetTransactionsByCustomerName(string customerName);

        public bool GetIsSecureByUsername(string username);
    }
}
