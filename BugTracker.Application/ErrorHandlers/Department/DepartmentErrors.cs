using BugTracker.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace Application.ErrorHandlers.Department
{
    public static class DepartmentErrors
    {
        public static readonly Error Duplicate = new Error("Department.Duplicate", "Department is already existed", StatusCodes.Status409Conflict);
        public static readonly Error NotFound = new Error("Department.NotFound", "Department is NotFound", StatusCodes.Status404NotFound);
        public static readonly Error NotEmpty = new Error("Department.NotEmpty", "Department is not empty", StatusCodes.Status404NotFound);
        public static readonly Error DepartmentHasNotThisSubjectOrReverse = new Error("Department.DepartmentHasNotThisSubjectOrReverse", "Department has not this subject or reverse", StatusCodes.Status404NotFound);

    }
}
