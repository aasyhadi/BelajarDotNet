using BelajarApi.Data;
using BelajarApi.Dtos;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Endpoints;

public static class DashboardEndpoint
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        app.MapGet("/dashboard", async (AppDbContext db) =>
        {
            var krsData = await db.Set<Krs>()
                .Include(x => x.MataKuliah)
                .ToListAsync();

            var favoriteCourse = krsData
                .GroupBy(x => x.MataKuliah.NamaMataKuliah)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();

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
                    .CountAsync(),

                TotalKrs = krsData.Count,

                MataKuliahTerfavorit = favoriteCourse?.Key ?? "-",

                JumlahPeminat = favoriteCourse?.Count() ?? 0
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