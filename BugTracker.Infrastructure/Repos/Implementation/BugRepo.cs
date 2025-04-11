using BugTracker.Data.models;
using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure.Repos.Implementation
{
    public class BugRepo : GenericRepo<Bug>, IBugRepo
    {
        private readonly AppDbContext _dbContext;
        public BugRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Bug> AddAsync(Bug entity)
        {
            await _dbContext.Bugs.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<Bug>> GetAllAsync()
        {
            return await _dbContext.Bugs
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Bug?> GetByIdAsync(int id)
        {
            return await _dbContext.Bugs
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<string> CheckIFBugExistAndUserToAssign(int bugId, int userId)
        {
            var bug = await _dbContext.Bugs.Include(x => x.userBugs).FirstOrDefaultAsync(b => b.Id == bugId);
            if (bug is null) return "Bug Not Found";

            if (bug.userBugs.Any(x => x.UserId == userId)) return "User Exists Already";
            try
            {
                bug.userBugs.Add(new UserBugs { UserId = userId });
                await _dbContext.SaveChangesAsync();
                return "Success";
            }
            catch
            {
                return "Error";
            }

        }

        public async Task<string> RemoveUserFromBug(int bugId, int userId)
        {
            var bug = await _dbContext.UserBugs
          .FirstOrDefaultAsync(ub => ub.BugId == bugId && ub.UserId == userId);
            if (bug is null) return "Not Found";

            try
            {
                _dbContext.UserBugs.Remove(bug);
                await _dbContext.SaveChangesAsync();
                return "Success";
            }
            catch
            {
                return "Error";
            }

        }



    }
}
