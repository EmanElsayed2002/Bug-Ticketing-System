namespace School.Application.DTOs.Authentication;

public record _RegisterRequest
(
    string Email,
    string Password,
    string FullName,
    List<string> Roles

);
