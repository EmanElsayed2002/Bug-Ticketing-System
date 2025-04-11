using BugTracker.Data.models;
using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure.Repos.Implementation
{
    public class ProjectRepo : GenericRepo<Project>, IProjectRepo
    {
        private readonly AppDbContext _dbContext;
        public ProjectRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<bool> ExistsAsync(int projectId)
        {
            return await _dbContext.Projects
                .AnyAsync(p => p.ProjectId == projectId);
        }
        public async Task<List<Project>> GetAllProjectsWithBugsAsync()
        {
            return await _dbContext.Projects
                .Include(p => p.Bugs)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Project> GetProjectIncludeBugsAsync(int projectId)
        {
            return await _dbContext.Projects
                .Include(p => p.Bugs)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }
    }
}
