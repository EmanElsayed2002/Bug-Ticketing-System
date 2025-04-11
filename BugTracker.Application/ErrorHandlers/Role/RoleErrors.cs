
using BugTracker.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace Application.ErrorHandlers.Role
{
    public static class RoleErrors
    {

        public static readonly Error DuplicateRole = new Error("Role.Duplicate", "The role already exists", StatusCodes.Status409Conflict);
        public static readonly Error NotFound = new Error("Role.NotFound", "Role not found", StatusCodes.Status404NotFound);
    }


}
