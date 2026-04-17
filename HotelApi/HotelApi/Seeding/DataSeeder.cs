using System.Text.Json;
using HotelApi.Data;
using HotelApi.Importing;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedHotelsAsync(ApplicationDbContext context, string jsonPath)
        {
            if (await context.Hotels.AnyAsync())
                return;

            if (!File.Exists(jsonPath))
                return;

            var json = await File.ReadAllTextAsync(jsonPath);

            var hotelsFromJson = ParseHotels(json);

            if (hotelsFromJson == null || hotelsFromJson.Count == 0)
                return;

            foreach (var item in hotelsFromJson)
            {
                var hotel = new Hotel
                {
                    ExternalId = item.Id,
                    Name = item.Name,
                    City = item.City,
                    Country = item.Country,
                    CountryCode = item.CountryCode,
                    Address = item.Address,
                    Rating = item.Rating,
                    Lat = item.Lat,
                    Lng = item.Lng,
                    PricePerNightTry = item.PricePerNightTry,
                    Currency = item.Currency,
                    PriceType = item.PriceType
                };

                foreach (var amenityName in item.Amenities
                             .Where(a => !string.IsNullOrWhiteSpace(a))
                             .Select(a => a.Trim())
                             .Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    var amenity = await context.Amenities
                        .FirstOrDefaultAsync(a => a.Name == amenityName);

                    if (amenity == null)
                    {
                        amenity = new Amenity { Name = amenityName };
                        context.Amenities.Add(amenity);
                        await context.SaveChangesAsync();
                    }

                    hotel.HotelAmenities.Add(new HotelAmenity
                    {
                        Hotel = hotel,
                        Amenity = amenity
                    });
                }

                foreach (var image in item.Images.Where(i => !string.IsNullOrWhiteSpace(i)))
                {
                    hotel.Images.Add(new HotelImage
                    {
                        ImageUrl = image
                    });
                }

                if (item.PhotoSources != null)
                {
                    hotel.PhotoSource = new PhotoSource
                    {
                        GoogleImagesSearch = item.PhotoSources.GoogleImagesSearch,
                        GoogleMapsSearch = item.PhotoSources.GoogleMapsSearch
                    };
                }

                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();
            }
        }

        private static List<ImportHotelJson>? ParseHotels(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<ImportHotelJson>>(json, options);
                }

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("data", out var dataElement))
                {
                    return JsonSerializer.Deserialize<List<ImportHotelJson>>(
                        dataElement.GetRawText(),
                        options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("JSON ERROR: " + ex.Message);
            }

            return new List<ImportHotelJson>();
        }
    }
}