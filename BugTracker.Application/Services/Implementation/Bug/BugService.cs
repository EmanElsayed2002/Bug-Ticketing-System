using AutoMapper;
using BugTracker.Application.DTOs.Bug.DTO;
using BugTracker.Application.ErrorHandlers.Bug;
using BugTracker.Application.ErrorHandlers.Project;
using BugTracker.Application.Errors;
using BugTracker.Application.Services.Abstract.Bugs;
using BugTracker.Infrastructure.Repos.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneOf;

namespace BugTracker.Application.Services.Implementation.Bug
{
    public class BugService : IBugService
    {
        private readonly IBugRepo _bugRepo;
        private readonly IMapper _maper;
        private readonly ILogger<BugService> _logger;
        private readonly IProjectRepo _projectRepo;

        public BugService(IBugRepo repo, IMapper mapper, ILogger<BugService> logger, IProjectRepo project)
        {
            _bugRepo = repo;
            _maper = mapper;
            _logger = logger;
            _projectRepo = project;
        }

        public async Task<OneOf<BugDtoResponse, Error>> AddNewBug(AddBugDTO request)
        {
            try
            {
                var projectExists = await _projectRepo.ExistsAsync(request.ProjectId);
                if (!projectExists)
                {
                    return ProjectsError.ProjectNotFound;
                }


                var bug = _maper.Map<Data.models.Bug>(request);
                var createdBug = await _bugRepo.AddAsync(bug);

                var result = _maper.Map<BugDtoResponse>(createdBug);
                return result;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating bug for project {ProjectId}", request.ProjectId);
                return BugError.DatabaseError;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating bug");
                return BugError.OperationFailed;
            }
        }

        public async Task<OneOf<Successes, Error>> AssignUserToBug(int bugId, int userId)
        {
            var res = await _bugRepo.CheckIFBugExistAndUserToAssign(bugId, userId);
            if (res == "Success") return new Successes("User Assigned To Bug Successfully ");
            return new Error(res, "Error in Assignment operation", StatusCodes.Status400BadRequest);
        }

        public async Task<OneOf<BugDtoResponse, Error>> BugDetails(int bugId)
        {
            try
            {
                var bug = await _bugRepo.GetByIdAsync(bugId);

                if (bug is null)
                {
                    return BugError.BugNotFound;
                }

                var result = _maper.Map<BugDtoResponse>(bug);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching bug with ID {bugId}");
                return BugError.DatabaseError;
            }
        }

        public async Task<OneOf<IEnumerable<BugDtoResponse>, Error>> GetAllBugs()
        {
            try
            {
                var bugs = await _bugRepo.GetAllAsync();



                var result = _maper.Map<IEnumerable<BugDtoResponse>>(bugs);
                return OneOf<IEnumerable<BugDtoResponse>, Error>.FromT0(result); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all bugs");
                return BugError.DatabaseError;
            }
        }

        public async Task<OneOf<Successes, Error>> UnassignUserFromBug(int bugId, int userId)
        {
            var res = await _bugRepo.RemoveUserFromBug(bugId, userId);
            if (res == "Success") return new Successes("User Unassigned To Bug Successfully");
            return new Error(res, "Error in Assignment operation", StatusCodes.Status400BadRequest);
        }
    }
}
