using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBank.Persistence.Dto;
using MyBank.Persistence.Services;

namespace MyBank.WebApi.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public TransactionController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("personal")]
        public async Task<IActionResult> Personal([FromBody] AddTakeOutMoneyDto request)
        {
            if (request.Amount <= 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            try
            {

                if (request.Type == "ADD")
                {
                    _customerService.AddMoneyById(request.AccountId, request.Amount);
                }
                else {
                    _customerService.TakeOutMoneyById(request.AccountId, request.Amount);
                }

                // ha sikeres volt az ellenőrzés
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
