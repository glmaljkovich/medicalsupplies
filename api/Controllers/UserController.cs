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
        [Route("signup")]
        public UserSignUpResponseDTO UserSignUp(UserSignUpRequestDTO userSignUpRequestDTO)
        { 
            _logger.LogInformation("UserSignUp email:" + userSignUpRequestDTO.Email);
            UserSignUpDTO userSignUpDTO = _mapper.Map<UserSignUpDTO>(userSignUpRequestDTO);
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
    }
}
