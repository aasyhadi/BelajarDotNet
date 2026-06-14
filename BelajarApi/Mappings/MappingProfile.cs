using AutoMapper;
using BelajarApi.Dtos;
using BelajarApi.Models;

namespace BelajarApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Mahasiswa, MahasiswaResponseDto>();
        CreateMap<MahasiswaDto, Mahasiswa>();

        CreateMap<MataKuliah, MataKuliahResponseDto>();
        CreateMap<MataKuliahDto, MataKuliah>();

        CreateMap<Mahasiswa, MahasiswaDetailResponseDto>();
    }
}