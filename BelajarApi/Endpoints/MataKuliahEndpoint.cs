using BelajarApi.Dtos;
using BelajarApi.Services;

namespace BelajarApi.Endpoints;

public static class MataKuliahEndpoint
{
    public static void MapMataKuliahEndpoints(this WebApplication app)
    {
        app.MapPost("/matakuliah", async (MataKuliahDto input, IMataKuliahService service) =>
        {
            var result = await service.CreateAsync(input);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Created($"/matakuliah/{result.Data?.Id}", result);
        });

        app.MapGet("/mahasiswa/{id}/matakuliah", async (int id, IMataKuliahService service) =>
        {
            var result = await service.GetByMahasiswaIdAsync(id);

            return Results.Ok(result);
        });

        app.MapGet("/matakuliah", async (IMataKuliahService service) =>
        {
            var result = await service.GetAllAsync();

            return Results.Ok(result);
        });

        app.MapDelete("/matakuliah/{id}", async (int id, IMataKuliahService service) =>
        {
            var result = await service.DeleteAsync(id);

            if (!result.Success)
            {
                return Results.NotFound(result);
            }

            return Results.Ok(result);
        });

    }

}