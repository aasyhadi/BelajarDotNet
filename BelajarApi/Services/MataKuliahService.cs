using BelajarApi.Dtos;
using BelajarApi.Models;
using BelajarApi.Repositories;
using AutoMapper;

namespace BelajarApi.Services;

public class MataKuliahService : IMataKuliahService
{
    private readonly IMataKuliahRepository _mataKuliahRepository;
    private readonly IMahasiswaRepository _mahasiswaRepository;
    private readonly IMapper _mapper;

    public MataKuliahService(
        IMataKuliahRepository mataKuliahRepository,
        IMahasiswaRepository mahasiswaRepository,
        IMapper mapper)
    {
        _mataKuliahRepository = mataKuliahRepository;
        _mahasiswaRepository = mahasiswaRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<MataKuliahResponseDto>> CreateAsync(MataKuliahDto input)
    {
        if (string.IsNullOrWhiteSpace(input.NamaMataKuliah))
        {
            return new ApiResponse<MataKuliahResponseDto>
            {
                Success = false,
                Message = "Nama mata kuliah wajib diisi"
            };
        }

        if (input.Sks <= 0)
        {
            return new ApiResponse<MataKuliahResponseDto>
            {
                Success = false,
                Message = "SKS harus lebih dari 0"
            };
        }

        var mahasiswa = await _mahasiswaRepository.GetByIdAsync(input.MahasiswaId);

        if (mahasiswa == null)
        {
            return new ApiResponse<MataKuliahResponseDto>
            {
                Success = false,
                Message = "Mahasiswa tidak ditemukan"
            };
        }

        var mataKuliah = _mapper.Map<MataKuliah>(input);

        var result = await _mataKuliahRepository.CreateAsync(mataKuliah);

        return new ApiResponse<MataKuliahResponseDto>
        {
            Success = true,
            Message = "Mata kuliah berhasil ditambahkan",
            Data = _mapper.Map<MataKuliahResponseDto>(result)
        };
    }

    public async Task<ApiResponse<List<MataKuliahResponseDto>>> GetByMahasiswaIdAsync(int mahasiswaId)
    {
        var data = await _mataKuliahRepository.GetByMahasiswaIdAsync(mahasiswaId);

        return new ApiResponse<List<MataKuliahResponseDto>>
        {
            Success = true,
            Message = "Data mata kuliah berhasil diambil",
            Data = _mapper.Map<List<MataKuliahResponseDto>>(data)
        };
    }

    public async Task<ApiResponse<List<MataKuliahResponseDto>>> GetAllAsync()
    {
        var data = await _mataKuliahRepository.GetAllAsync();

        return new ApiResponse<List<MataKuliahResponseDto>>
        {
            Success = true,
            Message = "Data mata kuliah berhasil diambil",
            Data = _mapper.Map<List<MataKuliahResponseDto>>(data)
        };
    }

    public async Task<ApiResponse<object>> DeleteAsync(int id)
    {
        var mataKuliah = await _mataKuliahRepository.GetByIdAsync(id);

        if (mataKuliah == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Mata kuliah tidak ditemukan"
            };
        }

        await _mataKuliahRepository.DeleteAsync(mataKuliah);

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Mata kuliah berhasil dihapus"
        };
    }
}