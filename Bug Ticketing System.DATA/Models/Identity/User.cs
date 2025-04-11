using Microsoft.AspNetCore.Identity;

namespace Bug_Ticketing_System.DATA.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsDisable { get; set; }
        public ICollection<RefreshToken> refreshTokens { get; set; }
    }
}
