using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class PhotoSource
    {
        public int PhotoSourceId { get; set; }

        [MaxLength(1000)]
        public string? GoogleImagesSearch { get; set; }

        [MaxLength(1000)]
        public string? GoogleMapsSearch { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
    }
}