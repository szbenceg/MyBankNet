using MyBank.Desktop.ViewModel;
using MyBank.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.Model
{
    public class MyBankApiService
    {
        private readonly HttpClient _client;

        private string _accessToken;
        private string _refreshToken;

        public MyBankApiService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<CustomerDto>> LoadCustomersAsync()
        {
            SetJWT();

            var response = await _client.GetAsync("/api/customer/customers");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<CustomerDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<IEnumerable<TransactionHistoryDto>> LoadTransactionsByCustomerId(int customerId)
        {
            SetJWT();

            var response = await _client.GetAsync("/api/customer/" + customerId + "/transactions");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<TransactionHistoryDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<Boolean> AddOrTakeOutMoneyAsync(int accountId, int amount, String type)
        {
            SetJWT();

            AddTakeOutMoneyDto request = new AddTakeOutMoneyDto();
            request.AccountId = accountId;
            request.Amount = amount;
            request.Type = type;

            var response = await _client.PostAsJsonAsync("/api/transaction/personal", request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<Boolean> CreateTransaction(TransactionViewModel requestParams)
        {
            SetJWT();

            CreateTransactionDto request = new CreateTransactionDto();
            request.SourceAccountNumber = requestParams.SourceAccountNumber;
            request.DestinationAccountNumber = requestParams.DestinationAccountNumber;
            request.Message = requestParams.Message;
            request.BenificaryName = requestParams.BenificaryName;
            try
            {
                request.TransactionTotal = Int32.Parse(requestParams.TransactionTotal);
            }
            catch (Exception ex)
            {
                return false;
            }

            var response = await _client.PostAsJsonAsync("/api/transaction/transaction", request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<Boolean> LockAccount(int accountId, bool isLocked)
        {
            SetJWT();

            if (accountId == 0)
            {
                return false;
            }
            var response = await _client.GetAsync("/api/account/" + accountId + "/lock/" + isLocked);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/auth/login", new LoginDto
                {
                    Username = userName,
                    Password = userPassword
                });

                try
                {
                    _accessToken = response.Headers.GetValues("Bearer").FirstOrDefault();
                }
                catch (InvalidOperationException)
                {
                    _accessToken = null;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void SetJWT()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<Boolean> Logout()
        {
            var response = await _client.GetAsync("/api/auth/logout");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }
    }
}
