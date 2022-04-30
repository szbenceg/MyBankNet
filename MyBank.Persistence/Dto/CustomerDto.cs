using MyBank.Persistence.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Persistence.Dto
{
    public class CustomerDto : IEquatable<CustomerDto>
    {
       
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<AccountDto> Accounts { get; set; }

        public bool Equals(CustomerDto? other)
        {
            bool isEquals = Id == other?.Id && Name == other?.Name;

            if (!isEquals)
            {
                return false;
            }

            if (other.Accounts.Count() != Accounts.Count)
            {
                return false;
            }

            for (int i = 0; i < other.Accounts.Count(); i++)
            {
                bool eq = Accounts.Contains(other.Accounts.ToArray()[i]);

                if (!eq)
                {
                    return false;
                }
            }

            return true;
        }

        public static explicit operator CustomerDto(Customer cust) => new CustomerDto
        {
            Name = cust.Name,
            Accounts = cust.Accounts.Select(account => (AccountDto)account).ToList(),
            Id = cust.Id,
        };
    }
}
