namespace BelajarApi.Models;

public class Mahasiswa
{
    public int Id { get; set; }

    public string Nama { get; set; } = "";

    public string Jurusan { get; set; } = "";

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public ICollection<MataKuliah> MataKuliahs { get; set; }
        = new List<MataKuliah>();
    
    public ICollection<Krs> KrsList { get; set;}
        = new List<Krs>();
}