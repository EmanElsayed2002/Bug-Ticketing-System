using BugTracker.Data.models.Identity;

namespace BugTracker.Data.models
{
    public class UserBugs
    {
        public int UserId { get; set; }
        public int BugId { get; set; }
        public User User { get; set; }
        public Bug Bug { get; set; }
    }
}
