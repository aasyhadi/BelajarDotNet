using BelajarApi.Models;

namespace BelajarApi.Repositories;

public interface IKrsRepository
{
    Task<List<Krs>> GetAllAsync();
    Task<Krs?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int mahasiswaId, int mataKuliahId);
    Task AddAsync(Krs krs);
    Task DeleteAsync(Krs krs);
}