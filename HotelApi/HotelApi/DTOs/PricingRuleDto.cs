namespace HotelApi.DTOs
{
    public class PricingRuleDto
    {
        public int PricingRuleId { get; set; }
        public int? HotelId { get; set; }
        public string HotelName { get; set; } = "All Hotels";
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Multiplier { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}