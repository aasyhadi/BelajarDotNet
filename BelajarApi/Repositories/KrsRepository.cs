using BelajarApi.Data;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Repositories;

public class KrsRepository : IKrsRepository
{
    private readonly AppDbContext _context;

    public KrsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Krs>> GetAllAsync()
    {
        return await _context.KrsList
            .Include(x => x.Mahasiswa)
            .Include(x => x.MataKuliah)
            .OrderByDescending(x => x.TanggalAmbil)
            .ToListAsync();
    }

    public async Task<Krs?> GetByIdAsync(int id)
    {
        return await _context.KrsList
            .Include(x => x.Mahasiswa)
            .Include(x => x.MataKuliah)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(int mahasiswaId, int mataKuliahId)
    {
        return await _context.KrsList.AnyAsync(x =>
            x.MahasiswaId == mahasiswaId &&
            x.MataKuliahId == mataKuliahId);
    }

    public async Task AddAsync(Krs krs)
    {
        _context.KrsList.Add(krs);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Krs krs)
    {
        _context.KrsList.Remove(krs);
        await _context.SaveChangesAsync();
    }
}