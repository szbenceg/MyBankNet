using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.ViewModel
{
    public class CustomEventArgs : EventArgs
    {
        public String Message { get; set; }

        public int AccountId { get; set; }

        public CustomEventArgs(String _message, int _accountId)
        {
            Message = _message;
            AccountId = _accountId;
        }
    }
}
