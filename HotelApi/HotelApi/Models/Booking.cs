using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string SpecialRequests { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}