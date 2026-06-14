namespace BelajarApi.Dtos;

public class MahasiswaQueryDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public string SortBy { get; set; } = "id";

    public string Direction { get; set; } = "asc";
}