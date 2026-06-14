using BelajarApi.Data;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _db;

    public AuditLogRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(AuditLog log)
    {
        _db.AuditLogs.Add(log);

        await _db.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _db.AuditLogs
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}