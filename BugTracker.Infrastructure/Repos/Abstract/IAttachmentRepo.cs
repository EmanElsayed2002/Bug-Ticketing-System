using BugTracker.Data.models;

namespace BugTracker.Infrastructure.Repos.Abstract
{
    public interface IAttachmentRepo : IGenericRepo<Attachment>
    {
        Task<IEnumerable<Attachment>> GetByBugIdAsync(int bugId);
    }
}
