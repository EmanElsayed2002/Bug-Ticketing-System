namespace BugTracker.Data.models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Bug> Bugs { get; set; }

    }
}
