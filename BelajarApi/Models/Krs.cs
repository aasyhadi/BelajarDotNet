namespace BelajarApi.Models;

public class Krs
{
    public int Id { get; set; }

    public int MahasiswaId { get; set; }

    public int MataKuliahId { get; set; }

    public DateTime TanggalAmbil { get; set; }
        = DateTime.UtcNow;

    public Mahasiswa Mahasiswa { get; set; } = null!;

    public MataKuliah MataKuliah { get; set; } = null!;
}