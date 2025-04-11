using BugTracker.Data.models;


namespace BugTracker.Infrastructure.Repos.Abstract
{
    public interface IBugRepo : IGenericRepo<Bug>
    {
        Task<Bug> AddAsync(Bug entity);
        Task<IEnumerable<Bug>> GetAllAsync();
        Task<Bug?> GetByIdAsync(int id);
        Task<string> CheckIFBugExistAndUserToAssign(int bugId, int userId);

        Task<string> RemoveUserFromBug(int bugId, int userId);
    }
}
