using BugTracker.Application.Errors;
using OneOf;
using School.Application.DTOs.Authentication;
using School.Application.DTOs.User;
using SurveyBasket.API.DTOs.User;

namespace BugTracker.Application.Services.Abstract.Auth
{
    public interface IAccountService
    {
        Task<UserProfileResponse> Profile(string userId);
        Task<Successes> UpdateAsync(string userId, UpdateProfileRequest request);
        Task<OneOf<Successes, Error>> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}
