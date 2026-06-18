namespace BelajarApi.Dtos;

public class KrsResponseDto
{
    public int Id { get; set; }

    public string NamaMahasiswa { get; set; }
        = string.Empty;

    public string NamaMataKuliah { get; set; }
        = string.Empty;

    public int Sks { get; set; }

    public DateTime TanggalAmbil { get; set; }
}