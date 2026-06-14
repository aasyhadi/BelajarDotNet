using BelajarApi.Dtos;
using BelajarApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BelajarApi.Endpoints;

public static class AuthEndpoint
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/register", async (RegisterDto input, IAuthService service) =>
        {
            var result = await service.RegisterAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        app.MapPost("/auth/login", async (LoginDto input, IAuthService service) =>
        {
            var result = await service.LoginAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        app.MapGet("/profile", (ClaimsPrincipal user) =>
        {
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Profile berhasil diambil",
                Data = new
                {
                    Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Name = user.FindFirst(ClaimTypes.Name)?.Value,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value,
                    Role = user.FindFirst(ClaimTypes.Role)?.Value
                }
            });
        })
        .RequireAuthorization();

        app.MapPost("/auth/register-admin", async (RegisterDto input, IAuthService service) =>
        {
            var result = await service.RegisterAdminAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        app.MapPost("/auth/refresh-token", async (RefreshTokenDto input, IAuthService service) =>
        {
            var result = await service.RefreshTokenAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        app.MapPost("/auth/logout", async (RefreshTokenDto input, IAuthService service) =>
        {
            var result = await service.LogoutAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });
    }
}