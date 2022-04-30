using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBank.Persistence.Services;

namespace MyBank.WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet("{accountId}/lock/{doesLock}")]
        public ActionResult<bool> LockAccount(int accountId, bool doesLock)
        {
            if (accountId <= 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool isSuccess = _customerService.LockAccountById(accountId, doesLock);

            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
