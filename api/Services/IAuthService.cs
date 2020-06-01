using ArqNetCore.DTOs.Auth;

namespace ArqNetCore.Services
{
    public interface IAuthService
    {
        
        AuthTokenResultDTO AuthToken(AuthTokenDTO authTokenDTO);

        void Verify(AuthVerifyDTO authTokenDTO);

        AuthHashResultDTO Hash(string valueRaw);

    }
}