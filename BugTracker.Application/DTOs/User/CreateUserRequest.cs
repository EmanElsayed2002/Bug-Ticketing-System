namespace School.Application.DTOs.User;

public record CreateUserRequest
(
    string FullName,
    string Email,
    string Password,
    IList<string> Roles
);
