using BugTracker.Data.models.Identity;

namespace BugTracker.Application.Services.Abstract.Auth
{
    public interface IJwtProvider
    {

        JwtResult GenerateTaken(User user, IEnumerable<string> roles);

        string? ValidateTaken(string taken);
    }
    public record JwtResult(string taken, int expireIn);
}
