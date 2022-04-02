using Microsoft.AspNetCore.Identity;

namespace MyBank.Model.Dao
{
    public class Customer : IdentityUser<int>
    {
        /* IdentityUser<T> contains:
         * T Id
         * string UserName
         * string PasswordHash (UserPassword helyett)
         * string Email
         * string PhoneNumber
         * string SecurityStamp (UserChallenge helyett)
         */
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; } = null!;

        public bool IsSecure { get; set; } = false;

        public string PinCode { get; set; } = null!;

        public ICollection<Account> Accounts { get; set; }
    }
}
