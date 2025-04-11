using BugTracker.Application.Errors;
using OneOf;

namespace BugTracker.Application.Services.Abstract.UserBug
{
    public interface IUserBugService
    {
        Task<OneOf<Error, Successes>> AssignUserToBug();
        Task<OneOf<Error, Successes>> RemoveUserFromBug();
    }
}
