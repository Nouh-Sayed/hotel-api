using System.Text.Json.Serialization;

namespace HotelApi.Importing
{
    public class ImportHotelJson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("amenities")]
        public List<string> Amenities { get; set; } = new();

        [JsonPropertyName("price_per_night_try")]
        public decimal PricePerNightTry { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "TRY";

        [JsonPropertyName("price_type")]
        public string PriceType { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public List<string> Images { get; set; } = new();

        [JsonPropertyName("photo_sources")]
        public ImportPhotoSource? PhotoSources { get; set; }
    }

    public class ImportPhotoSource
    {
        [JsonPropertyName("google_images_search")]
        public string? GoogleImagesSearch { get; set; }

        [JsonPropertyName("google_maps_search")]
        public string? GoogleMapsSearch { get; set; }
    }

    public class HotelImportWrapper
    {
        [JsonPropertyName("data")]
        public List<ImportHotelJson> Data { get; set; } = new();
    }
}