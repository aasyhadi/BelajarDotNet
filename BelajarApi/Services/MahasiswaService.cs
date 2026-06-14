using BelajarApi.Dtos;
using BelajarApi.Services;
using BelajarApi.Models;
using BelajarApi.Repositories;
using AutoMapper;

namespace BelajarApi.Services;

public class MahasiswaService : IMahasiswaService
{
    private readonly IMahasiswaRepository _repository;
    private readonly IMapper _mapper;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ICurrentUserService _currentUserService;

    public MahasiswaService(
        IMahasiswaRepository repository,
        IMapper mapper,
        IAuditLogRepository auditLogRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _mapper = mapper;
        _auditLogRepository = auditLogRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<List<MahasiswaResponseDto>>> GetAllAsync()
    {
        var mahasiswas = await _repository.GetAllAsync();

        return new ApiResponse<List<MahasiswaResponseDto>>
        {
            Success = true,
            Message = "Data mahasiswa berhasil diambil",
            Data = _mapper.Map<List<MahasiswaResponseDto>>(mahasiswas)
        };
    }

    public async Task<ApiResponse<MahasiswaResponseDto>> GetByIdAsync(int id)
    {
        var mahasiswa = await _repository.GetByIdAsync(id);

        if (mahasiswa == null)
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Data mahasiswa tidak ditemukan"
            };

        return new ApiResponse<MahasiswaResponseDto>
        {
            Success = true,
            Message = "Data mahasiswa berhasil ditemukan",
            Data = _mapper.Map<MahasiswaResponseDto>(mahasiswa)
        };
    }

    public async Task<ApiResponse<MahasiswaResponseDto>> CreateAsync(MahasiswaDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Nama))
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Nama wajib diisi"
            };

        if (string.IsNullOrWhiteSpace(input.Jurusan))
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Jurusan wajib diisi"
            };

        var mahasiswa = _mapper.Map<Mahasiswa>(input);

        var result = await _repository.CreateAsync(mahasiswa);

        await _auditLogRepository.CreateAsync(
        new AuditLog
        {
            UserEmail = _currentUserService.UserEmail ?? "SYSTEM",
            Action = "CREATE",
            EntityName = "Mahasiswa",
            EntityId = result.Id,
            Description =
                $"Menambah mahasiswa {result.Nama}",
            CreatedAt = DateTime.Now
        });

        return new ApiResponse<MahasiswaResponseDto>
        {
            Success = true,
            Message = "Data mahasiswa berhasil ditambahkan",
            Data = _mapper.Map<MahasiswaResponseDto>(result)
        };
    }

    public async Task<ApiResponse<MahasiswaResponseDto>> UpdateAsync(int id, MahasiswaDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Nama))
        {
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Nama wajib diisi"
            };
        }

        if (string.IsNullOrWhiteSpace(input.Jurusan))
        {
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Jurusan wajib diisi"
            };
        }

        var mahasiswa = await _repository.GetByIdAsync(id);

        if (mahasiswa == null)
        {
            return new ApiResponse<MahasiswaResponseDto>
            {
                Success = false,
                Message = "Data mahasiswa tidak ditemukan"
            };
        }

        _mapper.Map(input, mahasiswa);

        await _repository.UpdateAsync(mahasiswa);

        await _auditLogRepository.CreateAsync(
        new AuditLog
        {
            UserEmail = _currentUserService.UserEmail ?? "SYSTEM",
            Action = "UPDATE",
            EntityName = "Mahasiswa",
            EntityId = mahasiswa.Id,
            Description =
                $"Mengubah mahasiswa {mahasiswa.Nama}",
            CreatedAt = DateTime.Now
        });

        return new ApiResponse<MahasiswaResponseDto>
        {
            Success = true,
            Message = "Data mahasiswa berhasil diperbarui",
            Data = _mapper.Map<MahasiswaResponseDto>(mahasiswa)
        };
    }

    public async Task<ApiResponse<object>> DeleteAsync(int id)
    {
        var mahasiswa = await _repository.GetByIdAsync(id);

        if (mahasiswa == null)
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Data mahasiswa tidak ditemukan"
            };

        mahasiswa.IsDeleted = true;
        mahasiswa.DeletedAt = DateTime.Now;

        await _repository.UpdateAsync(mahasiswa);

        await _auditLogRepository.CreateAsync(
        new AuditLog
        {
            UserEmail = _currentUserService.UserEmail ?? "SYSTEM",
            Action = "DELETE",
            EntityName = "Mahasiswa",
            EntityId = mahasiswa.Id,
            Description =
                $"Menghapus mahasiswa {mahasiswa.Nama}",
            CreatedAt = DateTime.Now
        });

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Data mahasiswa berhasil dihapus"
        };
    }

    public async Task<ApiResponse<List<MahasiswaResponseDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new ApiResponse<List<MahasiswaResponseDto>>
            {
                Success = false,
                Message = "Keyword wajib diisi",
                Data = new List<MahasiswaResponseDto>()
            };
        }

        var mahasiswas = await _repository.SearchAsync(keyword);

        return new ApiResponse<List<MahasiswaResponseDto>>
        {
            Success = true,
            Message = "Data pencarian berhasil diambil",
            Data = _mapper.Map<List<MahasiswaResponseDto>>(mahasiswas)
        };
    }

    public async Task<PagedResponse<List<MahasiswaResponseDto>>> GetFilteredAsync(MahasiswaQueryDto query)
    {
        var result = await _repository.GetFilteredAsync(query);

        return new PagedResponse<List<MahasiswaResponseDto>>
        {
            Success = true,
            Message = "Data mahasiswa berhasil diambil",
            Page = query.Page,
            PageSize = query.PageSize,
            TotalData = result.TotalData,
            TotalPage = (int)Math.Ceiling(result.TotalData / (double)query.PageSize),
            Data = _mapper.Map<List<MahasiswaResponseDto>>(result.Data)
        };
    }

    public async Task<ApiResponse<MahasiswaDetailResponseDto>> GetDetailByIdAsync(int id)
    {
        var mahasiswa = await _repository.GetDetailByIdAsync(id);

        if (mahasiswa == null)
        {
            return new ApiResponse<MahasiswaDetailResponseDto>
            {
                Success = false,
                Message = "Mahasiswa tidak ditemukan"
            };
        }

        return new ApiResponse<MahasiswaDetailResponseDto>
        {
            Success = true,
            Message = "Detail mahasiswa berhasil diambil",
            Data = _mapper.Map<MahasiswaDetailResponseDto>(mahasiswa)
        };
    }

    public async Task<PagedResponse<List<MahasiswaResponseDto>>> GetPagedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var result = await _repository.GetPagedAsync(page, pageSize);
        var data = _mapper.Map<List<MahasiswaResponseDto>>(result.Data);

        return new PagedResponse<List<MahasiswaResponseDto>>
        {
            Success = true,
            Message = "Data mahasiswa berhasil diambil",
            Page = page,
            PageSize = pageSize,
            TotalData = result.TotalData,
            TotalPage = (int)Math.Ceiling(result.TotalData / (double)pageSize),
            Data = data
        };
    }

    public async Task<ApiResponse<List<MahasiswaResponseDto>>>
    GetSortedAsync(string sortBy, string direction)
    {
        var data = await _repository
            .GetSortedAsync(sortBy, direction);

        return new ApiResponse<List<MahasiswaResponseDto>>
        {
            Success = true,
            Message = "Data mahasiswa berhasil diurutkan",
            Data = _mapper.Map<List<MahasiswaResponseDto>>(data)
        };
    }

    
}