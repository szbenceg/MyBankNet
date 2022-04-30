using MyBank.Desktop.Model;
using MyBank.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly MyBankApiService _apiService;

        public DelegateCommand RefreshCustomerListCommand { get; set; }
        //public DelegateCommand SelectedAccountChangedCommand { get; set; }
        
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand AddMoneyCommand { get; private set; }
        public DelegateCommand TakeOutMoneyCommand { get; private set; }
        public DelegateCommand CreateTransactionCommand { get; private set; }
        public DelegateCommand LockAccountCommand { get; private set; }
        public DelegateCommand ReleaseAccountCommand { get; private set; }

        public event EventHandler AddMoneyEvent;
        public event EventHandler CreateTransactionEvent;
        public event EventHandler LogoutEvent;

        public MainViewModel(MyBankApiService apiService)
        {
            _apiService = apiService;
            RefreshCustomerListCommand = new DelegateCommand(_ => LoadCustomerListAsync());
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());
            SelectCommand = new DelegateCommand(_ => LoadItemsAsync(SelectedCustomer));
            AddMoneyCommand = new DelegateCommand(_ => AddOrTakeOutMoneyCommandHandler("ADD"));
            TakeOutMoneyCommand = new DelegateCommand(_ => AddOrTakeOutMoneyCommandHandler("TAKE_OUT"));
            CreateTransactionCommand = new DelegateCommand(_ => CreateTransactionCommandHandler());
            LockAccountCommand = new DelegateCommand(_ => LockAccountCommandHandler());
            ReleaseAccountCommand = new DelegateCommand(_ => ReleaseAccountCommandHandler());
        }

        private async void LogoutAsync()
        {
            try
            {
                bool isSuccess = await _apiService.Logout();
                if (isSuccess)
                {
                    if (LogoutEvent != null)
                        LogoutEvent(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void ReleaseAccountCommandHandler()
        {
            try
            {
                bool isSuccess = await _apiService.LockAccount(SelectedAccountId, false);
                if (isSuccess)
                {
                    RefreshTransactions();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void LockAccountCommandHandler()
        {
            try
            {
                bool isSuccess = await _apiService.LockAccount(SelectedAccountId, true);
                if (isSuccess)
                {
                    RefreshTransactions();
                }
            }
            catch (Exception ex) 
            { 
            
            }
           
        }

        private void CreateTransactionCommandHandler()
        {
            if (CreateTransactionEvent != null)
                CreateTransactionEvent(this, new CustomEventArgs(SelectedAccountNumber));
        }

        private void AddOrTakeOutMoneyCommandHandler(string type)
        {
            if (AddMoneyEvent != null)
                AddMoneyEvent(this, new CustomEventArgs(type, SelectedAccountId));
        }

        private async void LoadCustomerListAsync()
        {
            try
            {
                CustomerList = new ObservableCollection<CustomerViewModel>(
                    (await _apiService.LoadCustomersAsync()).Select(listItem => (CustomerViewModel)listItem));
                if (SelectedCustomerId != 0)
                { 
                    SelectedCustomer = CustomerList.Where(customerViewModel => customerViewModel.Id == SelectedCustomerId).FirstOrDefault();
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void RefreshTransactions()
        {
            LoadCustomerListAsync();
            LoadItemsAsync(SelectedCustomer);
        }

        private async void LoadItemsAsync(CustomerViewModel list)
        {
            if (list is null)
            {
                return;
            }

            try
            {

                SelectedCustomerName = list.Name;
                SelectedAccountNumber = list.SelectedAccount.AccountNumber;
                TransactionList = new ObservableCollection<TransactionViewModel>(
                    (await _apiService.LoadTransactionsByCustomerId(list.Id)).Select(listItem => (TransactionViewModel)listItem));
                SelectedAccountBalance = list.SelectedAccount.Balance.ToString("N", new CultureInfo("hu-HU")) + " HUF";
                SelectedAccountId = list.SelectedAccount.Id;
                SelectedCustomerId = list.Id;
                OnPropertyChanged(nameof(SelectedAccountBalance));
                SelectedAccountIsLocked = list.SelectedAccount.IsLocked.ToString();

            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private CustomerViewModel _selectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        private String _selectedCustomerName;

        public String SelectedCustomerName
        {
            get { return _selectedCustomerName; }
            set { _selectedCustomerName = value; OnPropertyChanged(); }
        }

        private String _selectedAccountNumber;

        public String SelectedAccountNumber
        {
            get { return _selectedAccountNumber; }
            set { _selectedAccountNumber = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CustomerViewModel> _customerList;

        public ObservableCollection<CustomerViewModel> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        private ObservableCollection<TransactionViewModel> _transactionList;

        public ObservableCollection<TransactionViewModel> TransactionList
        {
            get { return _transactionList; }
            set
            {
                _transactionList = value;
                OnPropertyChanged(nameof(TransactionList));
            }
        }

        private String _selectedAccountBalance;

        public String SelectedAccountBalance { 
            get { return _selectedAccountBalance; }
            set { _selectedAccountBalance = value; OnPropertyChanged(); }
        }

        public int SelectedAccountId { get; set; }
        public int SelectedCustomerId { get; set; }

        private String _selectedAccountIsLocked;

        public String SelectedAccountIsLocked
        {
            get { return _selectedAccountIsLocked; }
            set {
                if (value == "True")
                {
                    _selectedAccountIsLocked = "Zárolt";
                }
                else 
                { 
                    _selectedAccountIsLocked = "Nem zárolt";
                }
                OnPropertyChanged(); 
            }
        }

    }
}
