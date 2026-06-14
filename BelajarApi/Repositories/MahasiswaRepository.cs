using BelajarApi.Data;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;
using BelajarApi.Dtos;

namespace BelajarApi.Repositories;

public class MahasiswaRepository : IMahasiswaRepository
{
    private readonly AppDbContext _db;

    public MahasiswaRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Mahasiswa>> GetAllAsync()
    {
        return await _db.Mahasiswas
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Mahasiswa?> GetByIdAsync(int id)
    {
        return await _db.Mahasiswas
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<Mahasiswa> CreateAsync(Mahasiswa mahasiswa)
    {
        _db.Mahasiswas.Add(mahasiswa);
        await _db.SaveChangesAsync();

        return mahasiswa;
    }

    public async Task<bool> UpdateAsync(Mahasiswa mahasiswa)
    {
        _db.Mahasiswas.Update(mahasiswa);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Mahasiswa mahasiswa)
    {
        _db.Mahasiswas.Remove(mahasiswa);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Mahasiswa>> SearchAsync(string keyword)
    {
        return await _db.Mahasiswas
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Mahasiswa?> GetDetailByIdAsync(int id)
    {
        return await _db.Mahasiswas
            .Include(m => m.MataKuliahs)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<(List<Mahasiswa> Data, int TotalData)> GetPagedAsync(int page, int pageSize)
    {
        var query = _db.Mahasiswas
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        var totalData = await query.CountAsync();

        var data = await query
            .OrderBy(m => m.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalData);
    }

    public async Task<List<Mahasiswa>> GetSortedAsync(
    string sortBy,
    string direction)
    {
        var query = _db.Mahasiswas.AsQueryable();

        sortBy = sortBy.ToLower();
        direction = direction.ToLower();

        query = (sortBy, direction) switch
        {
            ("nama", "asc") =>
                query.OrderBy(x => x.Nama),

            ("nama", "desc") =>
                query.OrderByDescending(x => x.Nama),

            ("jurusan", "asc") =>
                query.OrderBy(x => x.Jurusan),

            ("jurusan", "desc") =>
                query.OrderByDescending(x => x.Jurusan),

            _ =>
                query.OrderBy(x => x.Id)
        };

        return await query.ToListAsync();
    }

    public async Task<(List<Mahasiswa> Data, int TotalData)>
    GetFilteredAsync(MahasiswaQueryDto query)
    {
        var dbQuery = _db.Mahasiswas
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                x.Nama.Contains(query.Search) ||
                x.Jurusan.Contains(query.Search));
        }

        // Sorting
        dbQuery = (query.SortBy.ToLower(), query.Direction.ToLower()) switch
        {
            ("nama", "asc") =>
                dbQuery.OrderBy(x => x.Nama),

            ("nama", "desc") =>
                dbQuery.OrderByDescending(x => x.Nama),

            ("jurusan", "asc") =>
                dbQuery.OrderBy(x => x.Jurusan),

            ("jurusan", "desc") =>
                dbQuery.OrderByDescending(x => x.Jurusan),

            _ =>
                dbQuery.OrderBy(x => x.Id)
        };

        var totalData = await dbQuery.CountAsync();

        var data = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (data, totalData);
    }

}