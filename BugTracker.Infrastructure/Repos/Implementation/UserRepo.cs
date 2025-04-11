using BugTracker.Data.models.Identity;
using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;

namespace BugTracker.Infrastructure.Repos.Implementation
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public UserRepo(AppDbContext context) : base(context)
        {

        }
    }
}
