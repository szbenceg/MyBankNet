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
        private String _sourceAccountNumber;
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

        public CreateTransactionViewModel(string sourceAccountNumber, MyBankApiService apiService)
        {
            _sourceAccountNumber = sourceAccountNumber;
            _apiService = apiService;

            ExitCommand = new DelegateCommand(param => OnExitApplication());
            CreateTransactionCommand = new DelegateCommand(param => CreateTransactionCommandHandler());
            EditedTransaction = new TransactionViewModel();
            EditedTransaction.SourceAccountNumber = _sourceAccountNumber;
        }

        private async void CreateTransactionCommandHandler()
        {
            try {
                bool isSuccess = await _apiService.CreateTransaction(EditedTransaction);
                if (ExitApplication != null && isSuccess)
                    ExitApplication(this, EventArgs.Empty);
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
