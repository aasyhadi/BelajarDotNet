namespace BelajarApi.Services;

public interface ICurrentUserService
{
    string? UserEmail { get; }
    string? UserName { get; }
    string? Role { get; }
}