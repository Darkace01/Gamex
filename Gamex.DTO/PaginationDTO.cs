namespace Gamex.DTO;

public class PaginationDTO<T>
{
    public decimal TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Items { get; set; }

    public PaginationDTO()
    {

    }

    public PaginationDTO(IEnumerable<T> items, decimal totalPages, int currentPage, int pageSize, int totalCount)
    {
        Items = items;
        TotalPages = totalPages;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
