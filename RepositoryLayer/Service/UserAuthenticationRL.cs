using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Middleware.TokenGeneration;
using Middleware.PasswordHashing;
using ModelLayer.DTO;
using Middleware.SMTP;


namespace RepositoryLayer.Service
{
    public class UserAuthenticationRL : IUserAuthenticationRL
    {
        private readonly AddressBookDBContext _dbContext;
        private readonly Jwt _jwt;
        private readonly IEmailServices _emailService;
        public UserAuthenticationRL(AddressBookDBContext dbContext, Jwt jwt, IEmailServices emailServices)
        {
            _dbContext = dbContext;
            _jwt = jwt;
            _emailService = emailServices;
        }
        public bool Checkuser(string Email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public UserEntity RegisterUserRL(UserEntity newUser)
        {
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            return newUser;
        }

        public (bool login, string token) LoginUserRL(string email, string password)
        {
            UserEntity user = _dbContext.Users.FirstOrDefault(e => e.Email == email);

            if (user != null)
            {
                bool PasswordMatch = PasswordHasher.VerifyPassword(password, user.Password);

                if (PasswordMatch)
                {
                    string token = _jwt.GenerateToken(user.UserId, user.FirstName, user.LastName, user.Email);
                    return (true, token);
                }
            }
            return (false, string.Empty);
        }

        public (bool Found, string token) CheckUserEmail(string email)
        {
            UserEntity user = _dbContext.Users.FirstOrDefault(e => e.Email == email);
            if (user != null)
            {
                string token = _jwt.GenerateResetPasswordToken(user.Email);
                _emailService.SendResetPasswordEmailAsync(email, token);
                return (true, token);
            }
            return (false, "no token generation");
        }

        public (bool status, string message) ResetPasswordRL(ResetPasswordDTO resetCredentials)
        {
            if(!_jwt.ValidateToken(resetCredentials.Token, out int userId, out string Email))
            {
                return (false, "token validation failed canot reset password");
                
            }
            var user = _dbContext.Users.FirstOrDefault(e => e.Email == Email);
            if(user == null)
            {
                return (false, "no user with that email found!!");
            }
            user.Password = PasswordHasher.HashPassword(resetCredentials.NewPassword);
            _dbContext.SaveChanges();
            return (true, "password is successfully changed for the given id");
        }
    }
}
