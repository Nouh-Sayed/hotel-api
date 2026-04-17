using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;

        [Required, MaxLength(50)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string RoomType { get; set; } = string.Empty;

        [Range(1, 20)]
        public int Capacity { get; set; }

        [Range(0, 999999999)]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}