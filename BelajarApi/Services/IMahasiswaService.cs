using BelajarApi.Dtos;
using BelajarApi.Services;

namespace BelajarApi.Services;

public interface IMahasiswaService
{
    Task<ApiResponse<List<MahasiswaResponseDto>>> GetAllAsync();
    Task<ApiResponse<MahasiswaResponseDto>> GetByIdAsync(int id);
    Task<ApiResponse<MahasiswaResponseDto>> CreateAsync(MahasiswaDto input);
    Task<ApiResponse<MahasiswaResponseDto>> UpdateAsync(int id, MahasiswaDto input);
    Task<ApiResponse<object>> DeleteAsync(int id);
    Task<ApiResponse<List<MahasiswaResponseDto>>> SearchAsync(string keyword);
    Task<ApiResponse<MahasiswaDetailResponseDto>> GetDetailByIdAsync(int id);
    Task<PagedResponse<List<MahasiswaResponseDto>>> GetPagedAsync(int page, int pageSize);
    Task<ApiResponse<List<MahasiswaResponseDto>>> GetSortedAsync(string sortBy, string direction);
    Task<PagedResponse<List<MahasiswaResponseDto>>> GetFilteredAsync(MahasiswaQueryDto query);
}