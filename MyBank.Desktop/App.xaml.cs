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
        public App()
        {
            Startup += Application_Startup;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _apiService = new MyBankApiService(ConfigurationManager.AppSettings["baseAddress"]);

            _mainViewModel = new MainViewModel(_apiService);

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainView.Show();

        }
    }
}
