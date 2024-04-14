namespace Web.Client;

internal record UserInfo
{
    public string UserId { get; init; }
    public string Email { get; init; }
}