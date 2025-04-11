using Application.ErrorHandlers.Role;
using AutoMapper;
using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Data.models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using School.Application.DTOs.User;


namespace BugTracker.Application.Services.Implementation.Auth
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;

            _mapper = mapper;

        }
        public async Task<OneOf<Successes, Error>> CreateAsync(RoleRequest request)
        {
            var role = await _roleManager.RoleExistsAsync(request.name);
            if (role) return RoleErrors.DuplicateRole;
            await _roleManager.CreateAsync(new Role { Name = request.name });
            return new Successes("Created successfully");
        }

        public async Task<OneOf<Successes, Error>> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return RoleErrors.NotFound;
            await _roleManager.DeleteAsync(role);
            return new Successes("Deleted successfully");
        }

        public async Task<OneOf<IEnumerable<RoleResponse>, Error>> GetAllAsync()
        {
            try
            {
                var roles = await _roleManager.Roles
                   .ToListAsync();
                var roleResponses = _mapper.Map<IEnumerable<RoleResponse>>(roles);
                return OneOf<IEnumerable<RoleResponse>, Error>.FromT0(roleResponses);
            }
            catch
            {
                return new Error("Error occured", "GEt All Roles Failed", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OneOf<Successes, Error>> UpdateAsync(string id, RoleRequest request)
        {
            var roleIsExisted = await _roleManager.Roles.AnyAsync(r => r.Name == request.name && r.Id.ToString() != id);
            if (roleIsExisted)
                return RoleErrors.DuplicateRole;

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return RoleErrors.NotFound;

            role.Name = request.name;
            await _roleManager.UpdateAsync(role);
            return new Successes("Updated successfully");
        }
    }
}
