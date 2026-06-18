using BelajarApi.Dtos;
using BelajarApi.Models;
using BelajarApi.Repositories;

namespace BelajarApi.Services;

public class KrsService : IKrsService
{
    private readonly IKrsRepository _repository;

    public KrsService(IKrsRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<KrsResponseDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new KrsResponseDto
        {
            Id = x.Id,
            NamaMahasiswa = x.Mahasiswa.Nama,
            NamaMataKuliah = x.MataKuliah.NamaMataKuliah,
            Sks = x.MataKuliah.Sks,
            TanggalAmbil = x.TanggalAmbil
        }).ToList();
    }

    public async Task<KrsResponseDto> CreateAsync(KrsDto dto)
    {
        var exists = await _repository.ExistsAsync(
            dto.MahasiswaId,
            dto.MataKuliahId
        );

        if (exists)
        {
            throw new Exception("Mata kuliah sudah diambil oleh mahasiswa ini.");
        }

        var krs = new Krs
        {
            MahasiswaId = dto.MahasiswaId,
            MataKuliahId = dto.MataKuliahId
        };

        await _repository.AddAsync(krs);

        var saved = await _repository.GetByIdAsync(krs.Id);

        return new KrsResponseDto
        {
            Id = saved!.Id,
            NamaMahasiswa = saved.Mahasiswa.Nama,
            NamaMataKuliah = saved.MataKuliah.NamaMataKuliah,
            Sks = saved.MataKuliah.Sks,
            TanggalAmbil = saved.TanggalAmbil
        };
    }

    public async Task DeleteAsync(int id)
    {
        var krs = await _repository.GetByIdAsync(id);

        if (krs == null)
        {
            throw new Exception("Data KRS tidak ditemukan.");
        }

        await _repository.DeleteAsync(krs);
    }
}