using BelajarApi.Data;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Repositories;

public class MataKuliahRepository : IMataKuliahRepository
{
    private readonly AppDbContext _db;

    public MataKuliahRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MataKuliah> CreateAsync(MataKuliah mataKuliah)
    {
        _db.MataKuliahs.Add(mataKuliah);
        await _db.SaveChangesAsync();

        return mataKuliah;
    }

    public async Task<List<MataKuliah>> GetByMahasiswaIdAsync(int mahasiswaId)
    {
        return await _db.MataKuliahs
            .Where(mk => mk.MahasiswaId == mahasiswaId)
            .ToListAsync();
    }

    public async Task<List<MataKuliah>> GetAllAsync()
    {
        return await _db.MataKuliahs.ToListAsync();
    }

    public async Task<MataKuliah?> GetByIdAsync(int id)
    {
        return await _db.MataKuliahs.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(MataKuliah mataKuliah)
    {
        _db.MataKuliahs.Remove(mataKuliah);
        await _db.SaveChangesAsync();

        return true;
    }
}