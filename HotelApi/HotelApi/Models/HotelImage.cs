using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class HotelImage
    {
        public int HotelImageId { get; set; }

        [Required, MaxLength(1000)]
        public string ImageUrl { get; set; } = string.Empty;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
    }
}