using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Auth;
using ArqNetCore.DTOs.Account;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;

namespace ArqNetCore.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private ArqNetCoreDbContext _dbContext;
        private IAuthService _authService;
        private IAccountService _accountService;

        public UserService(
            ILogger<UserService> logger,
            ArqNetCoreDbContext dbContext,
            IAuthService authService,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _logger = logger;
            _dbContext = dbContext;
            _authService = authService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserSignUpResultDTO UserSignUp(UserSignUpDTO userSignUpDTO)
        {
            _logger.LogInformation("UserSignUp email:" + userSignUpDTO.Email);
            string passwordRaw = userSignUpDTO.Password;
            AuthHashResultDTO authHashResultDTO = _authService.Hash(passwordRaw);
            Account account = new Account{
                Id = userSignUpDTO.Email,
                PasswordHash = authHashResultDTO.ValueHash,
                PasswordSalt = authHashResultDTO.ValueSalt
            };
            _dbContext.Accounts.Add(account);
            User user = new User{
                Account = account
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return new UserSignUpResultDTO();
        }

        public UserSignInResultDTO UserSignIn(UserSignInDTO userSignInDTO){
            AccountFindResultDTO accountFindResultDTO = _accountService.Find(userSignInDTO.Email);
            //TODO migrate to _accountService
            AuthVerifyDTO authVerifyDTO = new AuthVerifyDTO{
                ValueRaw = userSignInDTO.Password,
                ValueSalt = accountFindResultDTO.PasswordSalt,
                ValueHash = accountFindResultDTO.PasswordHash
            };
            _authService.Verify(authVerifyDTO);
            AuthTokenDTO authTokenDTO = new AuthTokenDTO{
                SubjectRaw = new Dictionary<string, string>
                {
                    [ClaimType.ID] = userSignInDTO.Email
                },
                Claims = new Dictionary<string, object>()
            };
            AuthTokenResultDTO authTokenResultDTO = _authService.AuthToken(authTokenDTO);
            return new UserSignInResultDTO{
                Token = authTokenResultDTO.Token
            };
        }

        public UserAuthContextDTO UserAuthContext(){
            ClaimsPrincipal claimsPrincipal = _httpContextAccessor.HttpContext.User;
            Claim idClaim = claimsPrincipal.FindFirst(ClaimType.ID);
            return new UserAuthContextDTO{
                Id = idClaim.Value
            };
        }
        protected class ClaimType
        {
            public static string ID { get; set; } = "id";
        }
    }

    
}