using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;
using MyBank.WebApi.Model;

namespace MyBank.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);
                if (!result.Succeeded)
                {
                    return Forbid();
                }

                var user = _userManager.FindByNameAsync(loginDto.Username).Result;
                SetAuthorizationTokens(user);

                // ha sikeres volt az ellenőrzés
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private void SetAuthorizationTokens(Employee user)
        {
            string bearer = _jwtService.GenerateJWTToken(user);

            Response.Headers.Add("Bearer", bearer);
        }
    }
}