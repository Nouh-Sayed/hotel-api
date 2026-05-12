namespace HotelApi.Models
{
    public class PricingRule
    {
        public int PricingRuleId { get; set; }
        public int? HotelId { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal Multiplier { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Hotel? Hotel { get; set; }
    }
}