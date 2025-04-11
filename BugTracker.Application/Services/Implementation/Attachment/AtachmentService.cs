using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.AttachFile;
using BugTracker.Data.models;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OneOf;
using Error = BugTracker.Application.Errors.Error;

namespace BugTracker.Application.Services.Implementation.Attachments
{
    public class AtachmentService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IAttachmentRepo _repo;

        public AtachmentService(IAttachmentRepo repo, IWebHostEnvironment env)
        {
            _env = env;
            _repo = repo;
        }

        public async Task<OneOf<Successes, Error>> DeleteAttachment(int attachmentId)
        {
            var attachment = await _repo.GetByIdAsync(attachmentId);

            if (attachment == null)
                return new Error("NotFound", "Attachment not found", StatusCodes.Status400BadRequest);

            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", $"bug_{attachment.BugId}");
            var filePath = Path.Combine(uploadFolder, attachment.FileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await _repo.DeleteAsync(attachment);

            return new Successes("Attachment deleted successfully");
        }

        public async Task<OneOf<IEnumerable<Data.models.Attachment>, Error>> GetAttachmentForBug(int bugId)
        {
            var attachments = await _repo.GetByBugIdAsync(bugId);

            if (!attachments.Any())
                return new Error("NotFound", "No attachments found for this bug", StatusCodes.Status400BadRequest);

            return OneOf<IEnumerable<Data.models.Attachment>, Error>.FromT0(attachments);

        }

        public async Task<OneOf<Successes, Error>> UploadAttachment(int bugId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new Error("File is empty", "Not Found", StatusCodes.Status400BadRequest);

            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", $"bug_{bugId}");
            Directory.CreateDirectory(uploadFolder);

            var filePath = Path.Combine(uploadFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                FileName = file.FileName,
                FileType = file.ContentType,
                FileSize = (int)file.Length,
                BugId = bugId
            };

            await _repo.AddAsync(attachment);

            return new Successes("File uploaded successfully");
        }
    }
}
