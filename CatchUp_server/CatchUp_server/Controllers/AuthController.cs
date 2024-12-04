using CatchUp_server.Interfaces;
using CatchUp_server.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatchUp_server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }
        
        private async Task<IActionResult> Register(RegisterViewModel registerViewModel, string role)
        {
            var result = await _authService.Register(registerViewModel, role);
            if (result.Succeeded)
            {
                return Ok();
            }
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

        [HttpPost("register")]
        public Task<IActionResult> RegisterUser([FromBody] RegisterViewModel registerViewModel)
        {
            return Register(registerViewModel, "user");
        }

        [HttpPost("register-admin")]//, Authorize(Roles = "admin")]
        public Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel registerViewModel)
        {
            return Register(registerViewModel, "admin");
        }

        /*[HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await _authService.Login(loginViewModel);
            if (result.Succeeded)
            {
                // Typically, you would issue a JWT token here.
                //return Ok();
            }
            return Unauthorized("Invalid login credentials.");
        }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var loginResult = await _authService.Login(loginViewModel);
            if (loginResult.Succeeded)
            {
                var user = await _authService.FindUserByUsernameOrEmailAsync(loginViewModel.UserNameOrEmail);

                if (user != null)
                {
                    var token = await _authService.GenerateJwtToken(user);
                    return Ok(new { token });
                }
            }
            return Unauthorized("Invalid login credentials.");
        }

        [HttpPost("request-new-password")]
        public async Task<IActionResult> RequestNewPassword(string email)
        {
            var result = await _authService.RequestNewPassword(email);
            if (!result.result.Succeeded)
            {
                var errors = result.result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors });
            }

            //var userId = result.user.Id;
            var resetLink = $"http://localhost:4200/reset-password/{email}/{WebUtility.UrlEncode(result.resetToken)}";

            var subject = "Password Reset Request";
            var body = $"<h4>Click <a href=\"{resetLink}\">here</a> to reset your password.</h4><p>This email is auto-generated. Please do not reply!</p>";

            await _emailService.SendEmailAsync(email, subject, body);

            return Ok(result.resetToken);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel request)
        {
            var result = await _authService.ResetPassword(request.Email, request.ResetToken);
            if (result.result.Succeeded)
            {
                return Ok(new { result.password });
            }

            var errors = result.result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

        [HttpPost("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel request)
        {
            var result = await _authService.ChangePassword(request);
            if (result.Succeeded)
            {
                return Ok();
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

    }
}
