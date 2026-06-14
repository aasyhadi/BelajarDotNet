using BelajarApi.Dtos;

namespace BelajarApi.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto input);

    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto input);
    Task<ApiResponse<AuthResponseDto>> RegisterAdminAsync(RegisterDto input);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto input);
    Task<ApiResponse<object>> LogoutAsync(RefreshTokenDto input);
}