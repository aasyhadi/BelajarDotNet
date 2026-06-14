using BelajarApi.Models;
using BelajarApi.Dtos;

namespace BelajarApi.Repositories;

public interface IMahasiswaRepository
{
    Task<List<Mahasiswa>> GetAllAsync();
    Task<Mahasiswa?> GetByIdAsync(int id);
    Task<Mahasiswa> CreateAsync(Mahasiswa mahasiswa);
    Task<bool> UpdateAsync(Mahasiswa mahasiswa);
    Task<bool> DeleteAsync(Mahasiswa mahasiswa);
    Task<List<Mahasiswa>> SearchAsync(string keyword);
    Task<Mahasiswa?> GetDetailByIdAsync(int id);
    Task<(List<Mahasiswa> Data, int TotalData)> GetPagedAsync(int page, int pageSize);
    Task<List<Mahasiswa>> GetSortedAsync(
        string sortBy,
        string direction
    );
    Task<(List<Mahasiswa> Data, int TotalData)> GetFilteredAsync(MahasiswaQueryDto query);
}