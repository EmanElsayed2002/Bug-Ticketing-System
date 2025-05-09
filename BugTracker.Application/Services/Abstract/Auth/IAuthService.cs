﻿using BugTracker.Application.Errors;
using OneOf;
using School.Application.DTOs.Authentication;

namespace BugTracker.Application.Services.Abstract.Auth
{
    public interface IAuthService
    {
        Task<OneOf<AuthResponse?, Error>> GetTokenAsync(string email, string password);
        Task<OneOf<AuthResponse?, Error>> GetRefreshTokenAsync(string token, string refreshToken);
        Task<OneOf<bool, Error>> RevokeRefreshTokenAsync(string token, string refreshToken);
        Task<OneOf<string?, Error>> RegisterAsync(_RegisterRequest reques);
        Task<OneOf<AuthResponse?, Error>> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<OneOf<string, Error>> ResendConfirmationEmailAsync(_ResendConfirmationEmailRequest request);
        Task<OneOf<string, Error>> SendResetPasswordAsync(ForgetPasswordRequest request);
        Task<OneOf<string, Error>> ResetPasswordAsync(_ResetPasswordRequest request);
    }

}
