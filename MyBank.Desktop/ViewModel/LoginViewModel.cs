using MyBank.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyBank.Desktop.ViewModel
{
    public class LoginViewModel
    {
        private readonly MyBankApiService _bankApiService;

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public event EventHandler ExitApplication;

        public event EventHandler LoginSuccess;

        public event EventHandler LoginFailed;

        public LoginViewModel(MyBankApiService apiService)
        {
            _bankApiService = apiService;

            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                // a bejelentkezéshez szükségünk van a jelszótároló vezérlőre, mivel a jelszó tulajdonság nem köthető
                Boolean result = await _bankApiService.LoginAsync(UserName, passwordBox.Password);

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (Exception)
            {
                OnLoginFailed();
            }
        }

        private void OnLoginSuccess()
        {
            if (LoginSuccess != null)
                LoginSuccess(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            if (LoginFailed != null)
                LoginFailed(this, EventArgs.Empty);
        }

    }
}
