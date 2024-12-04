using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;

namespace CatchUp_server.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterViewModel registerViewModel, string role);
        Task<SignInResult> Login(LoginViewModel loginViewModel);
        Task<User> FindUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<string> GenerateJwtToken(User user);
        Task<(IdentityResult result, User user, string resetToken)> RequestNewPassword(string email);
        Task<(IdentityResult result, string password)> ResetPassword(string email, string token);
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel request);
    }
}
