using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBank.Persistence.Dao;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyBank.WebApi.Model
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IOptions<JwtConfig> _configuration;

        public JwtService(UserManager<Employee> userManager, IOptions<JwtConfig> configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public string GenerateJWTToken(Employee user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // kitölti a vezérlőkben a User.Identity.Name értékét
                //new Claim(ClaimTypes.Email, user.Email),   // hozzáadjuk az email címet
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // hozzáadjuk a user ID-t
            };

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_configuration.Value.JwtExpireMinutes);
            var issuer = _configuration.Value.JwtIssuer;
            var audience = _configuration.Value.JwtIssuer;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = expires,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            // alternatíva a JWT token elkészítésére:
            /*
            jwtToken = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            */

            string tokenStr = tokenHandler.WriteToken(jwtToken);
            return tokenStr;
        }

    }
}
