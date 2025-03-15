using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.TokenGeneration
{
    public class Jwt
    {
        private readonly IConfiguration _config;
        public Jwt(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(int UserId, string FirstName, string LastName, string Email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", UserId.ToString()),
                new Claim("Firstname", FirstName),
                new Claim("Lastname", LastName),
                //new Claim("UserName", UserName),
                new Claim("Email", Email),
                //new Claim("Phone", Phone)
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
