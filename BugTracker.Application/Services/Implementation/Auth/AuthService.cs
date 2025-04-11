using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Data.models.Identity;
using Hangfire;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using OneOf;

using School.Application.DTOs.Authentication;
using School.Application.ErrorHandler;
using System.Security.Cryptography;
using System.Text;


namespace BugTracker.Application.Services.Implementation.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtProvider _jwtProvider;
        private readonly RoleManager<Role> _roleManager;

        public AuthService(IEmailSender emailSender, RoleManager<Role> role, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtProvider = jwtProvider;
            _roleManager = role;
        }
        public async Task<OneOf<AuthResponse?, Error>> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null) return UserErrors.InvalidCode;
            var code = request.Code;
            if (user.EmailConfirmed) return UserErrors.DuplicateConfirmed;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            }
            catch
            {
                return UserErrors.InvalidCode;
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Const.AppRoles.Developer);
                return UserErrors.Success;
            }

            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);

        }

        public async Task<OneOf<AuthResponse?, Error>> GetRefreshTokenAsync(string token, string refreshToken)
        {
            var userId = _jwtProvider.ValidateTaken(token);
            if (userId is null)
                return UserErrors.InvalidRefreshToken;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return UserErrors.InvalidRefreshToken;

            if (user.LockoutEnd > DateTime.UtcNow)
                return UserErrors.UserLockOut;

            var userRefreshToken = user.refreshTokens.SingleOrDefault(u => u.Token == refreshToken);
            if (userRefreshToken is null)
                return UserErrors.InvalidRefreshToken;
            userRefreshToken.RevokedIn = DateTime.UtcNow;

            var userRoles = await _userManager.GetRolesAsync(user);
            var (newToken, expiresIn) = _jwtProvider.GenerateTaken(user, userRoles);


            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(14);
            user.refreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiredIn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id.ToString(), user.Email, user.FullName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        }

        private static string GenerateRefreshToken() =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        public async Task<OneOf<AuthResponse?, Error>> GetTokenAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return UserErrors.InvalidCredential;
            var checkPassword = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (checkPassword.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var (token, expiredIn) = _jwtProvider.GenerateTaken(user, roles);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiredIn = DateTime.UtcNow.AddDays(14);
                user.refreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiredIn = refreshTokenExpiredIn
                });
                return new AuthResponse(user.Id.ToString(), user.Email, user.FullName, token, expiredIn, refreshToken, refreshTokenExpiredIn);
            }
            var erro = checkPassword.IsLockedOut ? UserErrors.UserLockOut : checkPassword.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredential;
            return erro;
        }

        public async Task<OneOf<string?, Error>> RegisterAsync(_RegisterRequest reques)
        {
            var user = await _userManager.FindByEmailAsync(reques.Email);
            if (user is not null) return UserErrors.DuplicateUser;
            var appUser = new User()
            {
                Email = reques.Email,
                UserName = reques.Email,
                FullName = reques.FullName,
            };
            var res = await _userManager.CreateAsync(appUser, reques.Password);
            if (res.Succeeded)
            {
                if (reques.Roles is not null && reques.Roles.Any())
                {
                    var allowedRoles = _roleManager.Roles.ToList();

                    if (reques.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                        return UserErrors.InvalidRole;
                    var roleResult = await _userManager.AddToRolesAsync(appUser, reques.Roles);
                    if (!roleResult.Succeeded)
                    {
                        var roleError = roleResult.Errors.First();
                        return new Error(roleError.Code, roleError.Description, StatusCodes.Status400BadRequest);
                    }
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                await SendEmailConfirmation(appUser, code);
                return UserErrors.SendEmail;
            }
            var error = res.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        public async Task<OneOf<string, Error>> ResendConfirmationEmailAsync(_ResendConfirmationEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return UserErrors.ResendEmail;
            if (user.EmailConfirmed)
                return UserErrors.DuplicateConfirmed;

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await SendEmailConfirmation(user, code);
            return UserErrors.ResendEmail;
        }

        public async Task<OneOf<string, Error>> ResetPasswordAsync(_ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return UserErrors.ResetPassword;
            if (!user.EmailConfirmed)
                return UserErrors.EmailNotConfirmed;
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await SendResetPassword(user, code);
            return UserErrors.ResetPassword;
        }

        public async Task<OneOf<bool, Error>> RevokeRefreshTokenAsync(string token, string refreshToken)
        {
            var userId = _jwtProvider.ValidateTaken(token);
            if (userId is null)
                return UserErrors.InvalidRefreshToken_Token;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return UserErrors.InvalidRefreshToken_Token;

            var userRefreshToken = user.refreshTokens.SingleOrDefault(u => u.Token == refreshToken);
            if (userRefreshToken is null)
                return UserErrors.InvalidRefreshToken_Token;
            userRefreshToken.RevokedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }
        private async Task SendEmailConfirmation(User user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var TempPath = $"{Directory.GetCurrentDirectory()}/Templates/EmailConfirmation.html";
            StreamReader streamReader = new StreamReader(TempPath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();
            body = body
                .Replace("[name]", $"{user.FullName} ")
                .Replace("[action_url]", $"{origin}/api/auth/confirm-email?userId={user.Id}&code={code}");

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Confirm your email", body));
            await Task.CompletedTask;
        }
        private async Task SendResetPassword(User user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var TempPath = $"{Directory.GetCurrentDirectory()}/Templates/ForgetPassword.html";
            StreamReader streamReader = new StreamReader(TempPath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();
            body = body
                .Replace("{{name}}", $"{user.FullName} ")
                .Replace("{{action_url}}", $"{origin}/api/auth/forget-password?email={user.Email}&code={code}");

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Reset your password", body));
            await Task.CompletedTask;
        }

        public async Task<OneOf<string, Error>> SendResetPasswordAsync(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return UserErrors.ResetPassword;
            if (!user.EmailConfirmed)
                return UserErrors.EmailNotConfirmed;
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await SendResetPassword(user, code);
            return UserErrors.ResetPassword;
        }
    }
}
