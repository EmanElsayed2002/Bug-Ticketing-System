using BugTracker.Application.DTOs.Bug.DTO;
using BugTracker.Application.Services.Abstract.Bugs;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        public BugController(IBugService service)
        {
            _bugService = service;
        }
        [HttpPost("create-bug")]
        public async Task<IActionResult> CreateBug(AddBugDTO request)
        {
            var res = await _bugService.AddNewBug(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpGet("get-all-bugs")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _bugService.GetAllBugs();
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpGet("get-bug-details")]
        public async Task<IActionResult> GetBugDetails(int id)
        {
            var res = await _bugService.BugDetails(id);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("Assign-user-to-bug")]
        public async Task<IActionResult> AssignUserToBug(int bugid, int userid)
        {
            var res = await _bugService.AssignUserToBug(bugid, userid);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpPost("Remove-user-from-bug")]
        public async Task<IActionResult> UnassignmentUserFromBug(int bugid, int userid)
        {
            var res = await _bugService.UnassignUserFromBug(bugid, userid);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
    }
}
