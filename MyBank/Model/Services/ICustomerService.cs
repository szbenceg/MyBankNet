using MyBank.Model.Dao;

namespace MyBank.Model.Services
{
    public interface ICustomerService
    {
        public IEnumerable<Account> GetAccountsAsync(string customerName);
    }
}
