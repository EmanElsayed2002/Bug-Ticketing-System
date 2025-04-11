using BugTracker.Application.DTOs.Bug.DTO;

namespace BugTracker.Application.DTOs.Projects.DTO
{
    public class ProjectDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public List<BugDtoResponse> Bugs { get; set; } = new();
    }
}
