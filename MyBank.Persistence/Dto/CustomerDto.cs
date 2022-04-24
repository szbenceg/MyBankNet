using MyBank.Persistence.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Persistence.Dto
{
    public class CustomerDto
    {
       

        public string Name { get; set; } = null!;

        public ICollection<AccountDto> Accounts { get; set; }

        public static explicit operator CustomerDto(Customer cust) => new CustomerDto
        {
            Name = cust.Name,
            Accounts = cust.Accounts.Select(account => (AccountDto)account).ToList(),
        };
    }
}
