namespace HotelApi.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card";
        public string PaymentStatus { get; set; } = "Pending";

        public string TransactionCode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}