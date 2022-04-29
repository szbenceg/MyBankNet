using MyBank.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private int _id;

        private String _name;
        private IEnumerable<AccountDto> _accounts;
        private AccountDto _selectedAccount;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public AccountDto SelectedAccount
        {
            get { return _selectedAccount; }
            set { _selectedAccount = value; OnPropertyChanged(); }
        }

        public IEnumerable<AccountDto> Accounts 
        { 
            get { return _accounts; }
            set { _accounts = value; OnPropertyChanged(); }
        }

        public static explicit operator CustomerViewModel(CustomerDto dto) => new CustomerViewModel
        {
            Name = dto.Name,
            Accounts = dto.Accounts,
            SelectedAccount = dto.Accounts.FirstOrDefault(),
            Id = dto.Id
        };

        //public static explicit operator CustomerDto(CustomerViewModel vm) => new CustomerDto
        //{
        //    Name = vm.Name,
        //};
    }
}
