namespace BelajarApi.Dtos;

public class DashboardResponseDto
{
    public int TotalMahasiswa { get; set; }

    public int TotalMataKuliah { get; set; }

    public int TotalUser { get; set; }

    public int TotalAuditLog { get; set; }

    public int TotalKrs { get; set; }

    public string MataKuliahTerfavorit { get; set; }= string.Empty;

    public int JumlahPeminat { get; set; }
}