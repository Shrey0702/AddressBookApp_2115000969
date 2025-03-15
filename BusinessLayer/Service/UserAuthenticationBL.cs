using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.Response;
using RepositoryLayer.Entity;
using ModelLayer.DTO;
using RepositoryLayer.Interface;
using Middleware.PasswordHashing;

namespace BusinessLayer.Service
{
    public class UserAuthenticationBL: IUserAuthenticationBL
    {
        private readonly IMapper _mapper;
        private readonly IUserAuthenticationRL _userAuthRL;
        public UserAuthenticationBL(IMapper mapper, IUserAuthenticationRL userAuthRL)
        {
            _mapper = mapper;
            _userAuthRL = userAuthRL;
        }


        public Response<RegisterResponseDTO> RegisterUserBL(UserRegistrationDTO newUser)
        {
            bool Existing = _userAuthRL.Checkuser(newUser.Email);
            if (!Existing)
            {
                string hashPass = PasswordHasher.HashPassword(newUser.Password);
                newUser.Password = hashPass;

                UserEntity newUserEntity = _mapper.Map<UserEntity>(newUser);

                UserEntity registeredUser = _userAuthRL.RegisterUserRL(newUserEntity);
                RegisterResponseDTO registerResponse = _mapper.Map<RegisterResponseDTO>(registeredUser);

                Response<RegisterResponseDTO> registerResponseBack = new Response<RegisterResponseDTO>();
                registerResponseBack.Success = true;
                registerResponseBack.Message = "User Registered Successfully";
                registerResponseBack.Data = registerResponse;
                return registerResponseBack;

            }
            Response<RegisterResponseDTO> registerResponseBack1 = new Response<RegisterResponseDTO>();
            registerResponseBack1.Success = false;
            registerResponseBack1.Message = "User Already Exists";

            RegisterResponseDTO registerResponseFailed = new RegisterResponseDTO();
            registerResponseFailed.Email = newUser.Email;

            registerResponseBack1.Data = registerResponseFailed;
            return registerResponseBack1;


        }

        public Response<string> LoginUserBL(LoginDTO loginCrediantials)
        {
            //(bool Found, string HashPass) = _userAuthRL.GetUserCredentialsRL(loginCrediantials.Email);

            (bool login, string token) = _userAuthRL.LoginUserRL(loginCrediantials.Email, loginCrediantials.Password);
            if (login)
            {

                Response<string> response = new Response<string>();
                response.Success = true;
                response.Message = "Login Successfull";
                response.Data = token;
                return response;
            }
            Response<string> response1 = new Response<string>();
            response1.Success = false;
            response1.Message = "Incorrect Email or Password";
            response1.Data = "No Token Generated";
            return response1;
        }

        public Response<string> ForgotPasswordBL(string email)
        {
            (bool Found, string token) = _userAuthRL.CheckUserEmail(email);
            if(Found)
            {
                Response<string> response = new Response<string>();
                response.Success = true;
                response.Message = "User Found use the returned token to reset your password";
                response.Data = token;
                return response;
            }
            Response<string> response1 = new Response<string>();
            response1.Success = false;
            response1.Message = "User Not Found";
            response1.Data = "No Token";
            return response1;
        }

        public Response<string> ResetPasswordBL(ResetPasswordDTO resetCredentials)
        {
            (bool status, string message) = _userAuthRL.ResetPasswordRL(resetCredentials);
            if (status)
            {
                Response<string> response = new Response<string>();
                response.Success = true;
                response.Message = message;
                response.Data = "Password Reset Successfully";
                return response;
            }
            else
            {
                Response<string> response = new Response<string>();
                response.Success = false;
                response.Message = message;
                response.Data = "Password Reset Failed";
                return response;
            }
        }
    }
}
