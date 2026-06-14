namespace BelajarApi.Dtos;

public class MataKuliahResponseDto
{
    public int Id { get; set; }

    public string NamaMataKuliah { get; set; } = "";

    public int Sks { get; set; }

    public int MahasiswaId { get; set; }
}