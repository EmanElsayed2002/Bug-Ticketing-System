using BugTracker.Application.DTOs.Projects.DTO;
using BugTracker.Application.Errors;
using OneOf;

namespace BugTracker.Application.Services.Abstract.Projects
{
    public interface IProjectService
    {
        Task<OneOf<ProjectDtoResponse, Error>> AddNewProject(ProjectDtoRequest request);
        Task<OneOf<IEnumerable<ProjectDtoResponse>, Error>> GetAllProjects();
        Task<OneOf<ProjectDtoResponse, Error>> ProjectDetails(int projectId);
    }
}
