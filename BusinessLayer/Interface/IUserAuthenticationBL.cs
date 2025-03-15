using ModelLayer.DTO;
using ModelLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserAuthenticationBL
    {
        Response<RegisterResponseDTO> RegisterUserBL(UserRegistrationDTO newUser);

        Response<string> LoginUserBL(LoginDTO loginCrediantials);

        Response<string> ForgotPasswordBL(string email);
        Response<string> ResetPasswordBL(ResetPasswordDTO resetCredentials);
    }
}
