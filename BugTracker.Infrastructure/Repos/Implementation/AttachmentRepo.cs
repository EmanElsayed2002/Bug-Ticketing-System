using BugTracker.Data.models;
using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure.Repos.Implementation
{
    public class AttachmentRepo : GenericRepo<Attachment>, IAttachmentRepo
    {
        private readonly AppDbContext _context;

        public AttachmentRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetByBugIdAsync(int bugId)
        {

            return await _context.Attachments
                .Where(a => a.BugId == bugId)
                .ToListAsync();
        }
    }
}
