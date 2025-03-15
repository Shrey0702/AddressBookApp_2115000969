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

        public string GenerateResetPasswordToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Shorter expiry for security
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool ValidateToken(string token, out int userId, out string email)
        {
            userId = 0;
            email = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var userIdClaim = principal.FindFirst("UserId");
                var emailClaim = principal.FindFirst(ClaimTypes.Email);

                if (userIdClaim != null)
                {
                    userId = int.Parse(userIdClaim.Value);
                }

                email = emailClaim?.Value;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
