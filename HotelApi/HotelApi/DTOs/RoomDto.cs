namespace HotelApi.DTOs
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }

        public string Description { get; set; } = string.Empty;
        public string BedType { get; set; } = string.Empty;
        public string ViewType { get; set; } = string.Empty;

        public bool HasBalcony { get; set; }
        public bool HasBreakfast { get; set; }
        public bool HasWifi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasTV { get; set; }
    }
}