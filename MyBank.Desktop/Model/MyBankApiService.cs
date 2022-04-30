using MyBank.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.Model
{
    public class MyBankApiService
    {
        private readonly HttpClient _client;

        public MyBankApiService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<CustomerDto>> LoadCustomersAsync()
        {
            var response = await _client.GetAsync("/api/customer/customers");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<CustomerDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<IEnumerable<TransactionHistoryDto>> LoadTransactionsByCustomerId(int customerId)
        {
            var response = await _client.GetAsync("/api/customer/" + customerId + "/transactions");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<TransactionHistoryDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<Boolean> AddOrTakeOutMoneyAsync(int accountId, int amount, String type)
        {

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
    }
}
