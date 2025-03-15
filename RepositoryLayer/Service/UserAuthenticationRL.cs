﻿using System;
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


namespace RepositoryLayer.Service
{
    public class UserAuthenticationRL : IUserAuthenticationRL
    {
        private readonly AddressBookDBContext _dbContext;
        private readonly Jwt _jwt;
        public UserAuthenticationRL(AddressBookDBContext dbContext, Jwt jwt)
        {
            _dbContext = dbContext;
            _jwt = jwt;
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
    }
}
