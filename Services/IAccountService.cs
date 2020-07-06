using ArqNetCore.DTOs.Account;

namespace ArqNetCore.Services
{
    public interface IAccountService
    {
        public AccountFindResultDTO Find(string id);

    }
}