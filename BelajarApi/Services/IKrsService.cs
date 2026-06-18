using BelajarApi.Dtos;

namespace BelajarApi.Services;

public interface IKrsService
{
    Task<List<KrsResponseDto>> GetAllAsync();
    Task<KrsResponseDto> CreateAsync(KrsDto dto);
    Task DeleteAsync(int id);
}