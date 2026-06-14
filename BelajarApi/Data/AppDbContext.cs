using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mahasiswa> Mahasiswas { get; set; }
    public DbSet<MataKuliah> MataKuliahs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}