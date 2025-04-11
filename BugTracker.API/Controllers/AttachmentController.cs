using BugTracker.Application.Services.Abstract.AttachFile;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IFileService _fileService;

        public AttachmentController(IFileService service)
        {
            _fileService = service;
        }
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> Delete(int bugId, int attachmentId)
        {
            var res = await _fileService.DeleteAttachment(attachmentId);

            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpGet]
        public async Task<IActionResult> GetAttachments(int bugId)
        {
            var res = await _fileService.GetAttachmentForBug(bugId);

            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpPost]
        public async Task<IActionResult> Upload(int bugId, IFormFile file)
        {
            var res = await _fileService.UploadAttachment(bugId, file);

            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
    }
}
