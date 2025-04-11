using Microsoft.AspNetCore.Identity;

namespace BugTracker.Data.models.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool IsDisable { get; set; }
        public ICollection<UserBugs> userBugs { get; set; }
        public ICollection<RefreshToken> refreshTokens { get; set; }
    }
}
