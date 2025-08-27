namespace Shared.Models;

public class PaginatedList<T>
{
    public IList<T> Items { get; private set; } = Array.Empty<T>();
    public int Page { get; private set; }
    public int Size { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public PaginatedList(IList<T> items, int totalCount, int page, int size)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        Size = size;
        TotalPages = (int)Math.Ceiling(totalCount / (double)size);
    }
}
