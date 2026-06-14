namespace BelajarApi.Dtos;

public class MahasiswaDetailResponseDto
{
    public int Id { get; set; }

    public string Nama { get; set; } = "";

    public string Jurusan { get; set; } = "";

    public List<MataKuliahResponseDto> MataKuliahs { get; set; } = new();
}