using BelajarApi.Dtos;
using BelajarApi.Models;
using BelajarApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace BelajarApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto input)
    {
        var existingUser = await _userRepository.GetByEmailAsync(input.Email);

        if (existingUser != null)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Email sudah terdaftar"
            };
        }

        var user = new User
        {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password),
            Role = "User"
        };

        await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(user);

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Register berhasil",
            Data = new AuthResponseDto
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto input)
    {
        var user = await _userRepository.GetByEmailAsync(input.Email);

        if (user == null)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Email atau password salah"
            };
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(input.Password, user.PasswordHash);

        if (!validPassword)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Email atau password salah"
            };
        }

        var token = GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Login berhasil",
            Data = new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secretKey = jwtSettings["SecretKey"]!;
        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;
        var expiryMinutes = Convert.ToInt32(jwtSettings["ExpiryMinutes"]);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAdminAsync(RegisterDto input)
    {
        var existingUser = await _userRepository.GetByEmailAsync(input.Email);

        if (existingUser != null)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Email sudah terdaftar"
            };
        }

        var user = new User
        {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password),
            Role = "Admin"
        };

        await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(user);
        
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Register admin berhasil",
            Data = new AuthResponseDto
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                RefreshToken = refreshToken
            }
        };
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto input)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(input.RefreshToken);

        if (user == null)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Refresh token tidak valid"
            };
        }

        if (user.RefreshTokenExpiryTime < DateTime.Now)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Refresh token sudah expired"
            };
        }

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Token berhasil diperbarui",
            Data = new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<ApiResponse<object>> LogoutAsync(RefreshTokenDto input)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(input.RefreshToken);

        if (user == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Refresh token tidak valid"
            };
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _userRepository.UpdateAsync(user);

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Logout berhasil"
        };
    }
}