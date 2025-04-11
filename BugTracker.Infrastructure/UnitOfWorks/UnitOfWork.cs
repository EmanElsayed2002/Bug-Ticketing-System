using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;

namespace BugTracker.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IAttachmentRepo _attachmentRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly IUserRepo _userRepo;
        private readonly IBugRepo _bugRepo;

        public UnitOfWork(AppDbContext context, IAttachmentRepo attachmentRepo, IProjectRepo projectRepo, IUserRepo userRepo, IBugRepo bugRepo)
        {
            _context = context;
            _attachmentRepo = attachmentRepo;
            _projectRepo = projectRepo;
            _userRepo = userRepo;
            _bugRepo = bugRepo;
        }
        public IAttachmentRepo attachmentRepo => _attachmentRepo;

        public IBugRepo bugRepo => _bugRepo;

        public IProjectRepo projectRepo => _projectRepo;
        public IUserRepo userRepo => _userRepo;

        public Task SavechangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
