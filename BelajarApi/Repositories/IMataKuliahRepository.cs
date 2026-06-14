using BelajarApi.Models;

namespace BelajarApi.Repositories;

public interface IMataKuliahRepository
{
    Task<List<MataKuliah>> GetAllAsync();
    Task<MataKuliah?> GetByIdAsync(int id);
    Task<MataKuliah> CreateAsync(MataKuliah mataKuliah);
    Task<List<MataKuliah>> GetByMahasiswaIdAsync(int mahasiswaId);
    Task<bool> DeleteAsync(MataKuliah mataKuliah);
}