namespace BelajarApi.Dtos;

public class PagedResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = "";

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalData { get; set; }

    public int TotalPage { get; set; }

    public T? Data { get; set; }
}