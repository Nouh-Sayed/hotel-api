using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(10)]
        public string CountryCode { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [Range(0, 5)]
        public int Rating { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        [Range(0, 999999999)]
        public decimal PricePerNightTry { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "TRY";

        [MaxLength(100)]
        public string PriceType { get; set; } = string.Empty;

        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
        public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
        public PhotoSource? PhotoSource { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}