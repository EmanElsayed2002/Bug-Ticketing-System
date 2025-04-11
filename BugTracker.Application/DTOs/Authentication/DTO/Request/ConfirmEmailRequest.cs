namespace School.Application.DTOs.Authentication;

public record ConfirmEmailRequest
{
    public string UserId { get; init; }
    public string Code { get; init; }

    public ConfirmEmailRequest(string userId, string code)
    {
        UserId = userId;
        Code = code;
    }
}
