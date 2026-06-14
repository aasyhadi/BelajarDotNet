using BelajarApi.Dtos;
using BelajarApi.Services;
using BelajarApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BelajarApi.Endpoints;

public static class MahasiswaEndpoint
{
    public static void MapMahasiswaEndpoints(this WebApplication app)
    {
        app.MapGet("/mahasiswa", async (IMahasiswaService service) =>
        {
            var result = await service.GetAllAsync();
            return Results.Ok(result);
        })
        .RequireAuthorization();

        app.MapGet("/mahasiswa/{id}/detail", async (int id, AppDbContext db) =>
        {
            var mahasiswa = await db.Mahasiswas
                .Include(m => m.MataKuliahs)
                .Where(m => m.Id == id)
                .Select(m => new MahasiswaDetailResponseDto
                {
                    Id = m.Id,
                    Nama = m.Nama,
                    Jurusan = m.Jurusan,
                    MataKuliahs = m.MataKuliahs.Select(mk => new MataKuliahResponseDto
                    {
                        Id = mk.Id,
                        NamaMataKuliah = mk.NamaMataKuliah,
                        Sks = mk.Sks,
                        MahasiswaId = mk.MahasiswaId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (mahasiswa == null)
            {
                return Results.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Mahasiswa tidak ditemukan"
                });
            }

            return Results.Ok(new ApiResponse<MahasiswaDetailResponseDto>
            {
                Success = true,
                Message = "Detail mahasiswa berhasil diambil",
                Data = mahasiswa
            });
        });

        app.MapPost("/mahasiswa", async (
            MahasiswaDto input,
            IValidator<MahasiswaDto> validator,
            IMahasiswaService service) =>
        {
            var validationResult = await validator.ValidateAsync(input);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = validationResult.Errors.First().ErrorMessage
                });
            }

            var result = await service.CreateAsync(input);

            if (!result.Success)
                return Results.BadRequest(result);

            return Results.Created($"/mahasiswa/{result.Data?.Id}", result);
        })
        .RequireAuthorization();

        app.MapGet("/mahasiswa/paged", async (int page, int pageSize, IMahasiswaService service) =>
        {
            var result = await service.GetPagedAsync(page, pageSize);
            return Results.Ok(result);
        });

        app.MapPut("/mahasiswa/{id}", async (int id, MahasiswaDto input, IMahasiswaService service) =>
        {
            if (string.IsNullOrWhiteSpace(input.Nama))
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Nama wajib diisi"
                });
            }

            if (string.IsNullOrWhiteSpace(input.Jurusan))
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Jurusan wajib diisi"
                });
            }

            var result = await service.UpdateAsync(id, input);

            if (!result.Success)
            {
                return Results.NotFound(result);
            }

            return Results.Ok(result);
        })
        .RequireAuthorization();;

        app.MapGet("/mahasiswa/search", async (string keyword, IMahasiswaService service) =>
        {
            var result = await service.SearchAsync(keyword);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        app.MapGet("/mahasiswa/sort",
        async (
            string sortBy,
            string direction,
            IMahasiswaService service) =>
        {
            var result =
                await service.GetSortedAsync(
                    sortBy,
                    direction);

            return Results.Ok(result);
        });

        app.MapDelete("/mahasiswa/{id}", async (int id, IMahasiswaService service) =>
        {
            var result = await service.DeleteAsync(id);

            if (!result.Success)
                return Results.NotFound(result);

            return Results.Ok(result);
        })
        .RequireAuthorization("AdminOnly");

        app.MapGet("/mahasiswa/filter", async (
            int? page,
            int? pageSize,
            string? search,
            string? sortBy,
            string? direction,
            IMahasiswaService service) =>
        {
            var query = new MahasiswaQueryDto
            {
                Page = page ?? 1,
                PageSize = pageSize ?? 10,
                Search = search,
                SortBy = sortBy ?? "id",
                Direction = direction ?? "asc"
            };

            var result = await service.GetFilteredAsync(query);

            return Results.Ok(result);
        });
    }
}