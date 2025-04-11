using BugTracker.Application.DTOs.Bug.DTO;
using BugTracker.Application.Errors;
using OneOf;

namespace BugTracker.Application.Services.Abstract.Bugs
{
    public interface IBugService
    {
        Task<OneOf<BugDtoResponse, Error>> AddNewBug(AddBugDTO request);
        Task<OneOf<IEnumerable<BugDtoResponse>, Error>> GetAllBugs();
        Task<OneOf<BugDtoResponse, Error>> BugDetails(int projectId);
        Task<OneOf<Successes, Error>> AssignUserToBug(int bugId, int userId);
        Task<OneOf<Successes, Error>> UnassignUserFromBug(int bugId, int userId);
    }
}
