namespace HotelApi.DTOs
{
    public class PricingResultDto
    {
        public int RoomId { get; set; }
        public decimal BasePricePerNight { get; set; }
        public int Nights { get; set; }
        public decimal FinalPricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public List<string> AppliedRules { get; set; } = new();
    }
}