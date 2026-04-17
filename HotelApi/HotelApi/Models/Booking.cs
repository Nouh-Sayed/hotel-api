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

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        [Range(1, 20)]
        public int Adults { get; set; }

        [Range(0, 20)]
        public int Children { get; set; }

        public decimal TotalPrice { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Confirmed";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}