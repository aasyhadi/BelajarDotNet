namespace BelajarApi.Models;

public class MataKuliah
{
    public int Id { get; set; }

    public string NamaMataKuliah { get; set; } = "";

    public int Sks { get; set; }

    public int MahasiswaId { get; set; }

    public Mahasiswa? Mahasiswa { get; set; }

    public ICollection<Krs> KrsList { get; set; }
        = new List<Krs>();
}