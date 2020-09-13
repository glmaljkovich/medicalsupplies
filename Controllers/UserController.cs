using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ArqNetCore.Services;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Account;
using ArqNetCore.DTOs.Auth;

namespace ArqNetCore.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IUserService _userService;
        private IAccountService _accountService;
        private IAuthService _authService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IUserService userService,
            IAccountService accountService,
            IAuthService authService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _accountService = accountService;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        public UserSignUpResponseDTO UserSignUp(UserSignUpRequestDTO userSignUpRequestDTO)
        { 
            _logger.LogInformation("UserSignUp email:" + userSignUpRequestDTO.Email);
            UserSignUpDTO userSignUpDTO = new UserSignUpDTO{
                FirstName = userSignUpRequestDTO.First_name,
                LastName = userSignUpRequestDTO.Last_name,
                Phone = userSignUpRequestDTO.Phone,
                Company = userSignUpRequestDTO.Company,
                Position = userSignUpRequestDTO.Position,
                Locality = userSignUpRequestDTO.Locality,
                Email = userSignUpRequestDTO.Email,
                Password = userSignUpRequestDTO.Password
            };
            UserSignUpResultDTO userSignUpResultDTO = _userService.UserSignUp(userSignUpDTO);
            UserSignUpResponseDTO userSignUpResponseDTO = _mapper.Map<UserSignUpResponseDTO>(userSignUpResultDTO);
            return userSignUpResponseDTO;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public UserSignInResponseDTO UserSignIn(UserSignInRequestDTO userSignInRequestDTO)
        { 
            _logger.LogInformation("UserSignIn email:" + userSignInRequestDTO.Email);
            UserSignInDTO userSignInDTO = _mapper.Map<UserSignInDTO>(userSignInRequestDTO);
            UserSignInResultDTO userSignInResultDTO = _userService.UserSignIn(userSignInDTO);
            return new UserSignInResponseDTO{
                Access_token = userSignInResultDTO.Token
            };
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public UserProfileResponseDTO UserProfile()
        { 
            _logger.LogInformation("UserProfile");
            UserProfileResultDTO userSignInResultDTO = _userService.UserProfile();
            return new UserProfileResponseDTO{
                First_name = userSignInResultDTO.FirstName,
                Last_name = userSignInResultDTO.LastName,
                Phone = userSignInResultDTO.Phone,
                Company = userSignInResultDTO.Company,
                Position = userSignInResultDTO.Position,
                Locality = userSignInResultDTO.Locality,
                Email = userSignInResultDTO.Email,
                Is_admin = userSignInResultDTO.IsAdmin
            };
        }
    }
}
