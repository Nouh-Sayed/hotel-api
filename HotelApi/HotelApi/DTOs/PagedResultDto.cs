namespace HotelApi.DTOs
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}