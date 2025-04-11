using BugTracker.Application.Errors;
using BugTracker.Data.models;
using Microsoft.AspNetCore.Http;
using OneOf;

namespace BugTracker.Application.Services.Abstract.AttachFile
{
    public interface IFileService
    {
        Task<OneOf<Successes, Error>> UploadAttachment(int bugId, IFormFile file);
        Task<OneOf<IEnumerable<Attachment>, Error>> GetAttachmentForBug(int bugId);
        Task<OneOf<Successes, Error>> DeleteAttachment(int attachmentId);
    }
}
