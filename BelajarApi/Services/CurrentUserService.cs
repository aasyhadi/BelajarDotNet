using System.Security.Claims;

namespace BelajarApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserEmail =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Email)?
            .Value;

    public string? UserName =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Name)?
            .Value;

    public string? Role =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Role)?
            .Value;
}