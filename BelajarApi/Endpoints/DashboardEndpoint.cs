using BelajarApi.Data;
using BelajarApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Endpoints;

public static class DashboardEndpoint
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        app.MapGet("/dashboard", async (AppDbContext db) =>
        {
            var data = new DashboardResponseDto
            {
                TotalMahasiswa = await db.Mahasiswas
                    .Where(x => !x.IsDeleted)
                    .CountAsync(),

                TotalMataKuliah = await db.MataKuliahs
                    .CountAsync(),

                TotalUser = await db.Users
                    .CountAsync(),

                TotalAuditLog = await db.AuditLogs
                    .CountAsync()
            };

            return Results.Ok(new ApiResponse<DashboardResponseDto>
            {
                Success = true,
                Message = "Data dashboard berhasil diambil",
                Data = data
            });
        })
        .RequireAuthorization();
    }
}