using MyBank.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.ViewModel
{
    public class CreateTransactionViewModel : ViewModelBase
    {
        private int _accountId;
        private MyBankApiService _apiService;

        public event EventHandler ExitApplication;

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand CreateTransactionCommand { get; private set; }

        private TransactionViewModel _editedTransaction;
        public TransactionViewModel EditedTransaction
        {
            get { return _editedTransaction; }
            set { _editedTransaction = value; OnPropertyChanged(); }
        }

        public CreateTransactionViewModel(int accountId, MyBankApiService _apiService)
        {
            _accountId = accountId;
            _apiService = _apiService;

            ExitCommand = new DelegateCommand(param => OnExitApplication());
            CreateTransactionCommand = new DelegateCommand(param => CreateTransactionCommandHandler());
            EditedTransaction = new TransactionViewModel();
        }

        private void CreateTransactionCommandHandler()
        {
            int a = 0;
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
    }
}
