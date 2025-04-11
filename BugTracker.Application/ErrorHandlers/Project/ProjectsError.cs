using BugTracker.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace BugTracker.Application.ErrorHandlers.Project
{
    public static class ProjectsError
    {
        public static readonly Error DatabaseError = new Error("Project.InvalidData", "Invalid Insert project Data", StatusCodes.Status400BadRequest);
        public static readonly Error ProjectNotFound = new Error("Project.NotFound", "ProjectNotFound", StatusCodes.Status400BadRequest);

    }
}
