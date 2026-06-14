using BelajarApi.Models;

namespace BelajarApi.Repositories;

public interface IAuditLogRepository
{
    Task CreateAsync(AuditLog log);
    Task<List<AuditLog>> GetAllAsync();
}