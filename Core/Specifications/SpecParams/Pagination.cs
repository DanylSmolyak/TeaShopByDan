namespace Core.SpecParams;

public class Pagination<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    public int TotalItems { get; set; }
    
    public IReadOnlyList<T> Data { get; set; } 

    public Pagination(int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItems = totalCount;
        Data = data;
    }
}