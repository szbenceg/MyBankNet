using MyBank.Desktop.Model;
using MyBank.Desktop.View;
using MyBank.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyBank.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MyBankApiService _apiService;

        private MainViewModel _mainViewModel;
        private MainWindow _mainView;

        private AddMoneyViewModel _addMoneyViewModel;
        private AddMoneyWindow _addMoneyWindow;

        private CreateTransactionViewModel _createTransactionViewModel;
        private CreateTransactionWindow _createTransactionWindow;

        public App()
        {
            Startup += Application_Startup;

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _apiService = new MyBankApiService(ConfigurationManager.AppSettings["baseAddress"]);

            _mainViewModel = new MainViewModel(_apiService);
            
            _mainViewModel.AddMoneyEvent += new EventHandler(AddMoneyEventHandler);
            _mainViewModel.CreateTransactionEvent += new EventHandler(CreateTransactionEventHandler);

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainView.Show();
        }

        private void CreateTransactionEventHandler(object? sender, EventArgs e)
        {
            int accountId = ((CustomEventArgs)e).AccountId;
            
            if (accountId != 0)
            {
                _createTransactionViewModel = new CreateTransactionViewModel( accountId, _apiService);
                _createTransactionViewModel.ExitApplication += new EventHandler(CreateTransactionView_ExitApplication);

                _createTransactionWindow = new CreateTransactionWindow();
                _createTransactionWindow.DataContext = _createTransactionViewModel;
                _createTransactionWindow.Show();
            }

        }

        private void CreateTransactionView_ExitApplication(object? sender, EventArgs e)
        {
            _createTransactionWindow.Close();
            _mainViewModel.RefreshTransactions();
        }

        private void AddMoneyEventHandler(object? sender, EventArgs e)
        {
            String type = ((CustomEventArgs)e).Message;
            int accountId = ((CustomEventArgs)e).AccountId;

            if (accountId != 0) {
                _addMoneyViewModel = new AddMoneyViewModel(type, accountId, _apiService);
                _addMoneyViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

                _addMoneyWindow = new AddMoneyWindow();
                _addMoneyWindow.DataContext = _addMoneyViewModel;
                _addMoneyWindow.Show();
            }
        }

        private void ViewModel_ExitApplication(object? sender, EventArgs e)
        {
            _addMoneyWindow.Close();
            _mainViewModel.RefreshTransactions();
        }
    }
}
