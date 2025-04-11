using BugTracker.Application.DTOs.Projects.DTO;
using BugTracker.Application.Services.Abstract.Projects;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpPost("add-project")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDtoRequest request)
        {
            var res = await _projectService.AddNewProject(request);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
        [HttpGet("get-all-projects")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _projectService.GetAllProjects();
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpGet("get-project-details")]
        public async Task<IActionResult> GetProjectDetails(int id)
        {
            var res = await _projectService.ProjectDetails(id);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));
        }
    }
}
