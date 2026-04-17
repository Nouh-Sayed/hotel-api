using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class CreateBookingDto
    {
        [Required]
        public int CustomerId { get; set; }

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