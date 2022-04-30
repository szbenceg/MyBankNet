using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Persistence.Dto
{
    public class AddTakeOutMoneyDto
    {
        public int Amount { get; set; }
        public int AccountId { get; set; }
        public String Type { get; set; }
    }
}
