using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Amenity
    {
        public int AmenityId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
    }
}