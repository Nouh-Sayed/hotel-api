using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Nationality { get; set; }

        [MaxLength(100)]
        public string? IdentityNumber { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}