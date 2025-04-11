using AutoMapper;
using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Data.models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using School.Application.DTOs.Authentication;
using School.Application.DTOs.User;
using School.Application.ErrorHandler;
using SurveyBasket.API.DTOs.User;

namespace BugTracker.Application.Services.Implementation.Auth
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _authManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountService(IAuthService authService, IMapper mapper, UserManager<User> userManager)
        {
            _authManager = userManager;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<OneOf<Successes, Error>> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _authManager.FindByIdAsync(userId);
            if (user is null)
            {
                return UserErrors.UserNotFound;
            }
            var result = await _authManager.ChangePasswordAsync(user, request.currentPassword, request.newPassword);
            if (result.Succeeded)
            {
                return new Successes("Password Successfully Changed");
            }
            var err = result.Errors.First();
            return new Error(err.Code, err.Description, StatusCodes.Status400BadRequest);
        }

        public async Task<UserProfileResponse> Profile(string userId)
        {
            var user = await _authManager.Users.Where(x => x.Id == int.Parse(userId)).SingleAsync();
            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task<Successes> UpdateAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _authManager.FindByIdAsync(userId);
            var newUser = _mapper.Map(request, user);
            await _authManager.UpdateAsync(newUser);
            return new Successes("Profile Updated Successfully");
        }
    }
}
