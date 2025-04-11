using BugTracker.Data.models;

namespace BugTracker.Application.DTOs.Bug.DTO
{
    public class AddBugDTO
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int ProjectId { get; set; }
        public BugStatus BugStatus { get; set; }
        public BugPriority BugPriority { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
