using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Nationality { get; set; }
        public string? IdentityNumber { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 20)]
        public int Adults { get; set; }

        [Range(0, 20)]
        public int Children { get; set; }
    }
}