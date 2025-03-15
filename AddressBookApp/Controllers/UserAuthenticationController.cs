using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using ModelLayer.Response;

namespace AddressBookApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationBL _userAuthBL;
        public UserAuthenticationController(IUserAuthenticationBL userAuthBL)
        {
            _userAuthBL = userAuthBL;
        }
        [HttpPost]
        [Route("/auth/register")]
        public ActionResult RegisterUser([FromBody] UserRegistrationDTO newUser)
        {
            Response<RegisterResponseDTO> newUserResponse = _userAuthBL.RegisterUserBL(newUser);
            return Ok(newUserResponse);
        }

        [HttpPost]
        [Route("/auth/login")]
        public ActionResult LoginUser([FromBody]LoginDTO loginDetails)
        {
            Response<string> response = _userAuthBL.LoginUserBL(loginDetails);
            return Ok(response);
        }

        [HttpPost]
        [Route("/auth/forgotpassword")]
        public ActionResult ForgotPassword([FromBody]string email)
        {
            Response<string> response = _userAuthBL.ForgotPasswordBL(email);
            return Ok(response);
        }

        [HttpPost]
        [Route("/auth/resetpassword")]
        public ActionResult ResetPassword([FromBody]ResetPasswordDTO resetCredentials)
        {
            Response<string> response = _userAuthBL.ResetPasswordBL(resetCredentials);
            return Ok(response);
        }
    }
}
