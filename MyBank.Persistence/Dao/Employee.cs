using Microsoft.AspNetCore.Identity;

namespace MyBank.Persistence.Dao
{
    public class Employee : IdentityUser<int>
    {
        /* IdentityUser<T> contains:
         * T Id
         * string UserName
         * string PasswordHash (UserPassword helyett)
         * string Email
         * string PhoneNumber
         * string SecurityStamp (UserChallenge helyett)
         */

        public Employee()
        {

        }

        public string Name { get; set; } = null!;

    }
}
