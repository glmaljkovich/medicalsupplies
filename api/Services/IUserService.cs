using ArqNetCore.DTOs.User;

namespace ArqNetCore.Services
{
    public interface IUserService
    {
        UserSignUpResultDTO UserSignUp(UserSignUpDTO userSignUpDTO);

        UserSignInResultDTO UserSignIn(UserSignInDTO userSignInDTO);
        
    }
}