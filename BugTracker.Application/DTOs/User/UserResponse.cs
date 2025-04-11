namespace School.Application.DTOs.User;

public record UserResponse
(
    string Id,
    string FullName,
    string Email,
    bool IsDisable,
    IEnumerable<string> Roles
);
