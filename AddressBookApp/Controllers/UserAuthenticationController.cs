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
        [Route("/register")]
        public ActionResult RegisterUser([FromBody] UserRegistrationDTO newUser)
        {
            Response<RegisterResponseDTO> newUserResponse = _userAuthBL.RegisterUserBL(newUser);
            return Ok(newUserResponse);
        }

        [HttpPost]
        [Route("/login")]
        public ActionResult LoginUser(LoginDTO loginDetails)
        {
            Response<string> response = _userAuthBL.LoginUserBL(loginDetails);
            return Ok(response);
        }
    }
}
