using BelajarApi.Data;
using BelajarApi.Dtos;
using BelajarApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Endpoints;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", async (AppDbContext db) =>
        {
            var users = await db.Users
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Email,
                    x.Role
                })
                .ToListAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Data user berhasil diambil",
                Data = users
            });
        })
        .RequireAuthorization("SuperAdminOnly");

        app.MapPost("/users", async (
            CreateUserDto dto,
            AppDbContext db,
            IPasswordHasher<User> passwordHasher) =>
        {
            var exists = await db.Users.AnyAsync(x => x.Email == dto.Email);

            if (exists)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email sudah terdaftar"
                });
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role
            };

            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User berhasil ditambahkan",
                Data = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                }
            });
        })
        .RequireAuthorization("SuperAdminOnly");

        app.MapPut("/users/{id}/role", async (
            int id,
            UpdateUserRoleDto dto,
            AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return Results.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User tidak ditemukan"
                });
            }

            user.Role = dto.Role;

            await db.SaveChangesAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Role user berhasil diupdate"
            });
        })
        .RequireAuthorization("SuperAdminOnly");

        app.MapPut("/users/{id}/reset-password", async (
            int id,
            ResetPasswordDto dto,
            AppDbContext db,
            IPasswordHasher<User> passwordHasher) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return Results.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User tidak ditemukan"
                });
            }

            user.PasswordHash = passwordHasher.HashPassword(user, dto.NewPassword);

            await db.SaveChangesAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password user berhasil direset"
            });
        })
        .RequireAuthorization("SuperAdminOnly");

        app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return Results.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User tidak ditemukan"
                });
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User berhasil dihapus"
            });
        })
        .RequireAuthorization("SuperAdminOnly");
    }
}