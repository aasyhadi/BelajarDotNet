using BelajarApi.Dtos;

namespace BelajarApi.Services;

public interface IMataKuliahService
{
    Task<ApiResponse<MataKuliahResponseDto>> CreateAsync(MataKuliahDto input);
    Task<ApiResponse<List<MataKuliahResponseDto>>> GetByMahasiswaIdAsync(int mahasiswaId);
    Task<ApiResponse<List<MataKuliahResponseDto>>> GetAllAsync();
    Task<ApiResponse<object>> DeleteAsync(int id);
}