using BugTracker.Application.DTOs.Bug.DTO;

namespace BugTracker.Application.DTOs.Projects.DTO
{
    public class ProjectDtoRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<AddBugDTO>? Bugs { get; set; } = new List<AddBugDTO>();
    }
}
