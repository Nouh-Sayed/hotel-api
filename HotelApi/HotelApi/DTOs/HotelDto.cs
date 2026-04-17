namespace HotelApi.DTOs
{
    public class HotelDto
    {
        public int HotelId { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Rating { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public decimal PricePerNightTry { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string PriceType { get; set; } = string.Empty;
        public List<string> Amenities { get; set; } = new();
        public List<string> Images { get; set; } = new();
        public string? GoogleImagesSearch { get; set; }
        public string? GoogleMapsSearch { get; set; }
    }
}