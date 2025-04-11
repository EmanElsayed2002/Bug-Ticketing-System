using BugTracker.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace BugTracker.Application.ErrorHandlers.Bug
{
    public static class BugError
    {
        public static readonly Error DatabaseError = new Error("Bug.InvalidData", "A database error occurred while processing the bug", StatusCodes.Status400BadRequest);
        public static readonly Error OperationFailed = new Error("Bug.OperationFailed", "The bug operation failed", StatusCodes.Status400BadRequest);
        public static readonly Error BugNotFound = new Error("Bug.BugNotFound", "The bug NotFound", StatusCodes.Status400BadRequest);

    }
}
