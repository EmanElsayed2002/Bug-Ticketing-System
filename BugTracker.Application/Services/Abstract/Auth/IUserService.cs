using BugTracker.Application.Errors;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using School.Application.DTOs.User;

namespace BugTracker.Application.Services.Abstract.Auth
{
    public interface IUserService
    {
        Task<OneOf<UserResponse, Error>> GetByIdAsync(string id);
        Task<OneOf<UserResponse, Error>> CreateAsync(CreateUserRequest request);
        Task<OneOf<UserResponse, Error>> UpdateAsync(string id, UpdateUserRequest request);
        Task<OneOf<UserResponse, Error>> ToggleStatus(string id);
        Task<OneOf<NoContentResult, Error>> UnLock(string id);
    }
}
