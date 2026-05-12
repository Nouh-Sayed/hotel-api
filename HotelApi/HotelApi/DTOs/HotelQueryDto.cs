namespace HotelApi.DTOs
{
    public class HotelQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        public string? Search { get; set; }
        public string? City { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? Rating { get; set; }

        public string? SortBy { get; set; }
    }
}