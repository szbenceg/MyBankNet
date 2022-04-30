﻿using MyBank.Persistence.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Persistence.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int Balance { get; set; }

        public string ?AccountNumber { get; set; }

        public Boolean IsLocked { get; set; }

        public static explicit operator AccountDto(Account account) => new AccountDto { 
            Id = account.Id,
            Balance = account.Balance,
            AccountNumber = account.AccountNumber,
            IsLocked = account.IsLocked,
        };
    }
}
