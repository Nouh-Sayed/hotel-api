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

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BedType { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ViewType { get; set; } = string.Empty;

        public bool HasBalcony { get; set; }
        public bool HasBreakfast { get; set; }
        public bool HasWifi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasTV { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}