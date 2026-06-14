using BelajarApi.Dtos;
using BelajarApi.Repositories;

namespace BelajarApi.Endpoints;

public static class AuditLogEndpoint
{
    public static void MapAuditLogEndpoints(this WebApplication app)
    {
        app.MapGet("/audit-logs", async (IAuditLogRepository repository) =>
        {
            var logs = await repository.GetAllAsync();

            var data = logs.Select(x => new AuditLogResponseDto
            {
                Id = x.Id,
                UserEmail = x.UserEmail,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToList();

            return Results.Ok(new ApiResponse<List<AuditLogResponseDto>>
            {
                Success = true,
                Message = "Data audit log berhasil diambil",
                Data = data
            });
        })
        .RequireAuthorization("AdminOnly");
    }
}