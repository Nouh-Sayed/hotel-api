namespace HotelApi.DTOs
{
    public class PricingCalculationDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}