using System.ComponentModel.DataAnnotations;

namespace BugTracker.Data.models
{
    public class Bug
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public BugStatus BugStatus { get; set; }
        public BugPriority BugPriority { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<UserBugs> userBugs { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
