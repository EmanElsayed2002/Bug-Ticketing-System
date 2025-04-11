using BugTracker.Infrastructure.Repos.Abstract;

namespace BugTracker.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public IAttachmentRepo attachmentRepo { get; }
        public IBugRepo bugRepo { get; }
        public IProjectRepo projectRepo { get; }
        public IUserRepo userRepo { get; }
        public Task SavechangesAsync();

    }
}
