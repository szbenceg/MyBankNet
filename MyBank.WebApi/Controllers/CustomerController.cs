using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;
using MyBank.Persistence.Services;

namespace MyBank.WebApi.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        [Authorize]
        [HttpGet("customers")]
        public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
        {
            return Ok(_customerService.FindAllWithAccounts().Select(customer => (CustomerDto)customer));
        }

        [Authorize]
        [HttpGet("{customerId}/transactions")]
        public ActionResult<IEnumerable<TransactionHistoryDto>> GetTrasaction(int customerId)
        {
            return Ok(_customerService.GetTransactionsByCustomerId(customerId));
        }
    }
}
