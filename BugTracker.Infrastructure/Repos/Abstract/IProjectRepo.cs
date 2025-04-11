using BugTracker.Data.models;

namespace BugTracker.Infrastructure.Repos.Abstract
{
    public interface IProjectRepo : IGenericRepo<Project>
    {
        Task<List<Project>> GetAllProjectsWithBugsAsync();
        Task<Project> GetProjectIncludeBugsAsync(int projectId);
        Task<bool> ExistsAsync(int projectId);
    }
}
