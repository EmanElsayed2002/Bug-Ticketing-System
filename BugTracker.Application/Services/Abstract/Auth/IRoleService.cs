using BugTracker.Application.Errors;
using OneOf;
using School.Application.DTOs.User;

namespace BugTracker.Application.Services.Abstract.Auth
{
    public interface IRoleService
    {
        Task<OneOf<IEnumerable<RoleResponse>, Error>> GetAllAsync();
        Task<OneOf<Successes, Error>> CreateAsync(RoleRequest request);
        Task<OneOf<Successes, Error>> UpdateAsync(string id, RoleRequest request);
        Task<OneOf<Successes, Error>> DeleteAsync(string id);
    }
}
