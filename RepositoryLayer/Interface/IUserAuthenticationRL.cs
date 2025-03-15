using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserAuthenticationRL
    {
        bool Checkuser(string Email);

        UserEntity RegisterUserRL(UserEntity newUser);

        //(bool found, string hashPass) GetUserCredentialsRL(string email);

        (bool login, string token) LoginUserRL(string email, string password);
        (bool Found, string token) CheckUserEmail(string email);
        (bool status, string message) ResetPasswordRL(ResetPasswordDTO resetCredentials);
    }
}
