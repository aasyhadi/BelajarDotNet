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
    public DbSet<Krs> KrsList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Krs>()
            .HasOne(x => x.Mahasiswa)
            .WithMany(x => x.KrsList)
            .HasForeignKey(x => x.MahasiswaId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Krs>()
            .HasOne(x => x.MataKuliah)
            .WithMany(x => x.KrsList)
            .HasForeignKey(x => x.MataKuliahId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}