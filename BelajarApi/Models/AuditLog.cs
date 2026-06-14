namespace BelajarApi.Models;

public class AuditLog
{
    public int Id { get; set; }

    public string UserEmail { get; set; } = "";

    public string Action { get; set; } = "";

    public string EntityName { get; set; } = "";

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}