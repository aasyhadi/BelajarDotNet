using BelajarApi.Dtos;
using BelajarApi.Services;

namespace BelajarApi.Endpoints;

public static class KrsEndpoint
{
    public static void MapKrsEndpoints(this WebApplication app)
    {
        app.MapGet("/krs", async (IKrsService service) =>
        {
            var data = await service.GetAllAsync();

            return Results.Ok(new ApiResponse<List<KrsResponseDto>>
            {
                Success = true,
                Message = "Data KRS berhasil diambil",
                Data = data
            });
        })
        .RequireAuthorization();

        app.MapPost("/krs", async (
            KrsDto dto,
            IKrsService service) =>
        {
            var result = await service.CreateAsync(dto);

            return Results.Ok(new ApiResponse<KrsResponseDto>
            {
                Success = true,
                Message = "KRS berhasil ditambahkan",
                Data = result
            });
        })
        .RequireAuthorization();

        app.MapDelete("/krs/{id}", async (
            int id,
            IKrsService service) =>
        {
            await service.DeleteAsync(id);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "KRS berhasil dihapus",
                Data = null
            });
        })
        .RequireAuthorization();
    }
}