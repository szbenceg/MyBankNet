using MyBank.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyBank.Desktop.ViewModel
{
    public class AddMoneyViewModel : ViewModelBase
    {
        private readonly MyBankApiService apiService;
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand ExecuteCommand { get; private set; }
        
        public event EventHandler ExitApplication;

        private int _accountId;
        private String _methodType;
        public AddMoneyViewModel(String methodType, int accountId, MyBankApiService _apiService)
        {
            apiService = _apiService;
            _accountId = accountId;
            _methodType = methodType;
            ExitCommand = new DelegateCommand(param => OnExitApplication());
            ExecuteCommand = new DelegateCommand(param => ExecuteCommandhandler(param));
        }

        private async void ExecuteCommandhandler(object param)
        {
            try
            {
                String amount = ((TextBox)param).Text;
                await apiService.AddOrTakeOutMoneyAsync(_accountId, Convert.ToInt32(amount), _methodType);
                OnExitApplication();
            
            }
            catch (Exception ex) 
            {
            
            }
           
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
    }
}