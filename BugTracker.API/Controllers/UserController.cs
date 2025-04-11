using BugTracker.Application.Services.Abstract.Auth;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Authentication;

namespace BugTracker.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _auth;

        public UserController(IAuthService auth)
        {
            _auth = auth;
        }
        [HttpPost("/api/users/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] _RegisterRequest request)
        {
            var res = await _auth.RegisterAsync(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            var request = new ConfirmEmailRequest
            (
                 userId,
                 code
            );
            var res = await _auth.ConfirmEmailAsync(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }

        [HttpPost("resend-emailConfirmation")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] _ResendConfirmationEmailRequest request)
        {
            var res = await _auth.ResendConfirmationEmailAsync(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> forgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var res = await _auth.SendResetPasswordAsync(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] _ResetPasswordRequest request)
        {
            var res = await _auth.ResetPasswordAsync(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpPost("/api/users/login")]
        public async Task<IActionResult> Login(_LoginRequest request)
        {
            var res = await _auth.GetTokenAsync(request.email, request.password);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> GetRefreshToken(string token, string refreshToken)
        {
            var res = await _auth.GetRefreshTokenAsync(token, refreshToken);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTakenRequest request)
        {
            var res = await _auth.RevokeRefreshTokenAsync(request.Taken, request.RefreshTaken);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }


    }
}
