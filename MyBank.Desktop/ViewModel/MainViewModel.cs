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

        

        public MainViewModel(MyBankApiService apiService)
        {
            _apiService = apiService;
            RefreshCustomerListCommand = new DelegateCommand(_ => LoadCustomerListAsync());
            //LogoutCommand = new DelegateCommand(_ => LogoutAsync());
            SelectCommand = new DelegateCommand(_ => LoadItemsAsync(SelectedCustomer));
            AddMoneyCommand = new DelegateCommand(_ => AddMoneyCommandHandler());
            //SelectedAccountChangedCommand = new DelegateCommand(_ => SelectedAccountChangedHandler());
        }

        private void AddMoneyCommandHandler()
        {
            throw new NotImplementedException();
        }

        private async void LoadCustomerListAsync()
        {
            try
            {
                CustomerList = new ObservableCollection<CustomerViewModel>(
                    (await _apiService.LoadCustomersAsync()).Select(listItem => (CustomerViewModel)listItem));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
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
                //SelectedListName = list.Name;
                //Items = new ObservableCollection<ItemViewModel>((await _service.LoadItemsAsync(list.Id))
                //    .Select(item => (ItemViewModel)item));

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

    }
}
