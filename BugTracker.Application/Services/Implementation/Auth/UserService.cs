using AutoMapper;
using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Data.models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using School.Application.DTOs.User;
using School.Application.ErrorHandler;

namespace BugTracker.Application.Services.Implementation.Auth
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private IMapper _mapper;

        public UserService(UserManager<User> user, RoleManager<Role> role, IMapper Mapper)
        {
            _userManager = user;
            _roleManager = role;
            _mapper = Mapper;
        }
        public async Task<OneOf<UserResponse, Error>> CreateAsync(CreateUserRequest request)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email);
            if (emailIsExists)
                return UserErrors.DuplicateUser;

            var allowedRoles = await _roleManager.Roles.ToListAsync();
            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return UserErrors.InvalidRole;

            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user!, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, request.Roles);
                var response = new UserResponse(
               Id: user.Id.ToString(),
               FullName: user.FullName,
               Email: user.Email,
               IsDisable: user.IsDisable,
               Roles: request.Roles
           );

                return response;
            }
            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        public async Task<OneOf<UserResponse, Error>> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return UserErrors.UserNotFound;
            var userRoles = await _userManager.GetRolesAsync(user);
            var response = new UserResponse(
            Id: id,
            FullName: user.FullName,
            Email: user.Email,
            IsDisable: user.IsDisable,
            Roles: userRoles
        );

            return response;

        }

        public async Task<OneOf<UserResponse, Error>> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return UserErrors.UserNotFound;
            user.IsDisable = !user.IsDisable;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                var response = new UserResponse(
               Id: user.Id.ToString(),
               FullName: user.FullName,
               Email: user.Email,
               IsDisable: user.IsDisable,
               Roles: Roles
           );

                return response;
            }

            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        public async Task<OneOf<NoContentResult, Error>> UnLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return UserErrors.UserNotFound;

            await _userManager.SetLockoutEndDateAsync(user, null);
            return new NoContentResult();
        }

        public async Task<OneOf<UserResponse, Error>> UpdateAsync(string id, UpdateUserRequest request)
        {
            var emailIsExisted = await _userManager.Users.AnyAsync(u => u.Id != int.Parse(id) && u.Email == request.Email);
            if (emailIsExisted)
                return UserErrors.DuplicateUser;

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return UserErrors.UserNotFound;

            var allowedRoles = await _roleManager.Roles.ToListAsync();
            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return UserErrors.InvalidRole;

            var newUser = _mapper.Map(request, user);


            var result = await _userManager.UpdateAsync(newUser);
            if (result.Succeeded)
            {
                var oldRoles = await _userManager.GetRolesAsync(user);

                var res = await _userManager.AddToRolesAsync(newUser, request.Roles);

                var response = new UserResponse(
                Id: user.Id.ToString(),
                FullName: user.FullName,
                Email: user.Email,
                IsDisable: user.IsDisable,
                Roles: request.Roles
            );

                return response;
            }
            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }
    }
}
