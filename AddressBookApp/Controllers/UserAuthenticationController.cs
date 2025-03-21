using BusinessLayer.Interface;
using BusinessLayer.Service;
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
        private readonly IRabbitMQService _rabbitMQService;
        public UserAuthenticationController(IUserAuthenticationBL userAuthBL, IRabbitMQService rabbitMQService)
        {
            _userAuthBL = userAuthBL;
            _rabbitMQService = rabbitMQService;
        }
        [HttpPost]
        [Route("/auth/register")]
        public ActionResult RegisterUser([FromBody] UserRegistrationDTO newUser)
        {
            Response<RegisterResponseDTO> newUserResponse = _userAuthBL.RegisterUserBL(newUser);
            _rabbitMQService.SendMessage($"{newUser.Email}, You have successfully Registered!");
            return Ok(newUserResponse);
        }

        [HttpPost]
        [Route("/auth/login")]
        public ActionResult LoginUser([FromBody]LoginDTO loginDetails)
        {
            Response<string> response = _userAuthBL.LoginUserBL(loginDetails);
            _rabbitMQService.ReceiveMessage();
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
