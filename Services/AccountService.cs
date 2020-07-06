using Microsoft.Extensions.Logging;
using ArqNetCore.DTOs.Account;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;

namespace ArqNetCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<UserService> _logger;
        private ArqNetCoreDbContext _dbContext;

        public AccountService(
            ILogger<UserService> logger,
            ArqNetCoreDbContext dbContext
        )
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public AccountFindResultDTO Find(string id){
            _logger.LogInformation("Find id: " + id);
            Account result = _dbContext.Accounts.Find(id);
            return new AccountFindResultDTO{
                Id = result.Id,
                PasswordHash = result.PasswordHash,
                PasswordSalt = result.PasswordSalt
            };
        }


    }
}