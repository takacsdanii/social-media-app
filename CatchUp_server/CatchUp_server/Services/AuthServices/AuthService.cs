using CatchUp_server.Db;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.AuthViewModels;
using CatchUp_server.Services.UserContentServices;

namespace CatchUp_server.Services.AuthServices
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserProfileService _userProfileService;

        private const int minAge = 14;

        public AuthService(IConfiguration configuration, UserManager<User> userManager,
                            SignInManager<User> signInManager, UserProfileService userContentService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _userProfileService = userContentService;
        }


        private bool IsOldEnough(RegisterViewModel registerViewModel)
        {
            var today = DateTime.Today;
            var age = today.Year - registerViewModel.BirthDate.Year;

            if (registerViewModel.BirthDate.Date > today.AddYears(-age))
                age--;

            return age >= minAge;
        }


        public async Task<IdentityResult> Register(RegisterViewModel registerViewModel, string role)
        {
            if (!IsOldEnough(registerViewModel))
            {
                return IdentityResult.Failed(new IdentityError { Description = "You are not old enough to register!" });
            }

            if (registerViewModel.Password != registerViewModel.PasswordConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The given passwords do not match!" });
            }

            var existingUser = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "The username is already in use." });

            existingUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "The email is already in use." });

            var user = new User
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
                UserName = registerViewModel.UserName,
                BirthDate = registerViewModel.BirthDate,
                Gender = registerViewModel.Gender,
                RegisteredAt = DateTime.Now
            };

            _userProfileService.setDefaultProfilePic(user);
            _userProfileService.setDefaultCoverPic(user);

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<SignInResult> Login(LoginViewModel loginViewModel)
        {
            var user = await FindUserByUsernameOrEmailAsync(loginViewModel.UserNameOrEmail);

            if (user != null)
                return await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, false, false);

            return SignInResult.Failed;
        }

        public async Task<User> FindUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail) ??
                       await _userManager.FindByEmailAsync(usernameOrEmail);

            return user;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(userClaims);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            if (key.KeySize < 256)
            {
                throw new ArgumentOutOfRangeException("Jwt:Key", "JWT key must be at least 256 bits long (32 characters).");
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(IdentityResult result, User user, string resetToken)> RequestNewPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "User not found." }), null, null);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return (IdentityResult.Success, user, token);
        }

        public async Task<(IdentityResult result, string password)> ResetPassword(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "User not found." }), null);
            }

            string newPassword = GeneratePassword();

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return (result, newPassword);
        }


        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, request.oldPassword, false);
            if (!passwordCheck.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Old password is incorrect." });
            }

            if (request.newPassword != request.confirmNewPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "New passwords do not match." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resut = await _userManager.ResetPasswordAsync(user, token, request.newPassword);

            return resut;
        }

        private string GeneratePassword()
        {
            var random = new Random();
            int length = random.Next(8, 51);

            char[] chars = new char[length];

            char bigLetter = (char)random.Next(65, 91);
            int bigLetterPos = random.Next(length);

            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)random.Next(33, 127);
            }
            chars[bigLetterPos] = bigLetter;

            return new string(chars);
        }
    }
}
