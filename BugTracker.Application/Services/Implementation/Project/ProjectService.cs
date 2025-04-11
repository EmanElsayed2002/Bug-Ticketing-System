using AutoMapper;
using BugTracker.Application.DTOs.Projects.DTO;
using BugTracker.Application.ErrorHandlers.Project;
using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Projects;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.Extensions.Logging;
using OneOf;

namespace BugTracker.Application.Services.Implementation.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IProjectRepo projectRepo, IMapper mapper, ILogger<ProjectService> logger)
        {
            _projectRepo = projectRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<OneOf<ProjectDtoResponse, Error>> AddNewProject(ProjectDtoRequest request)
        {
            var project = _mapper.Map<Data.models.Project>(request);

            await _projectRepo.BeginTransactionAsync();
            try
            {

                var p = await _projectRepo.AddAsync(project);
                var newProj = _mapper.Map<ProjectDtoResponse>(p);
                await _projectRepo.CommitAsync();
                return newProj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new project");
                await _projectRepo.RollBackAsync();
                return ProjectsError.DatabaseError;
            }
        }

        public async Task<OneOf<IEnumerable<ProjectDtoResponse>, Error>> GetAllProjects()
        {
            try
            {
                var projects = await _projectRepo.GetAllProjectsWithBugsAsync();
                var projectDtos = _mapper.Map<IEnumerable<ProjectDtoResponse>>(projects);
                return OneOf<IEnumerable<ProjectDtoResponse>, Error>.FromT0(projectDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all projects");
                return ProjectsError.DatabaseError;
            }
        }

        public async Task<OneOf<ProjectDtoResponse, Error>> ProjectDetails(int projectId)
        {
            try
            {
                var project = await _projectRepo.GetProjectIncludeBugsAsync(projectId);

                if (project is null)
                {
                    return ProjectsError.ProjectNotFound;
                }

                var projectDto = _mapper.Map<ProjectDtoResponse>(project);
                return projectDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching project with ID {projectId}");
                return ProjectsError.DatabaseError;
            }
        }
    }
}
