using HotelApi.Data;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Seeding
{
    public static class RoomSeeder
    {
        public static async Task SeedRoomsAsync(ApplicationDbContext context)
        {
            var hotels = await context.Hotels
                .AsNoTracking()
                .ToListAsync();

            foreach (var hotel in hotels)
            {
                var hasRooms = await context.Rooms.AnyAsync(r => r.HotelId == hotel.HotelId);
                if (hasRooms)
                    continue;

                var rooms = GenerateRoomsForHotel(hotel);
                context.Rooms.AddRange(rooms);
            }

            await context.SaveChangesAsync();
        }

        private static List<Room> GenerateRoomsForHotel(Hotel hotel)
        {
            var rooms = new List<Room>();
            var basePrice = hotel.PricePerNightTry <= 0 ? 2000 : hotel.PricePerNightTry;

            int totalRooms;
            if (hotel.Rating >= 5)
                totalRooms = 24;
            else if (hotel.Rating == 4)
                totalRooms = 18;
            else if (hotel.Rating == 3)
                totalRooms = 14;
            else
                totalRooms = 10;

            int floor = 1;
            int roomCounter = 1;

            for (int i = 0; i < totalRooms; i++)
            {
                if (roomCounter > 8)
                {
                    floor++;
                    roomCounter = 1;
                }

                var roomNumber = $"{floor}{roomCounter:00}";
                var roomType = GetRoomType(hotel.Rating, i, totalRooms);
                var capacity = GetCapacity(roomType);
                var price = CalculateRoomPrice(basePrice, roomType);

                rooms.Add(new Room
                {
                    HotelId = hotel.HotelId,
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    Capacity = capacity,
                    PricePerNight = price,
                    IsAvailable = true,
                    Description = GetDescription(roomType),
                    BedType = GetBedType(roomType),
                    ViewType = GetViewType(hotel.Rating, i),
                    HasBalcony = HasBalcony(roomType),
                    HasBreakfast = HasBreakfast(hotel.Rating),
                    HasWifi = true,
                    HasAirConditioning = hotel.Rating >= 3,
                    HasTV = true
                });

                roomCounter++;
            }

            return rooms;
        }

        private static string GetRoomType(int rating, int index, int totalRooms)
        {
            if (rating >= 5)
            {
                if (index >= totalRooms - 2) return "Presidential Suite";
                if (index >= totalRooms - 5) return "Executive Suite";
                if (index >= totalRooms - 9) return "Family Suite";
                if (index % 5 == 0) return "Deluxe Double";
                if (index % 3 == 0) return "Twin Room";
                return "Standard Double";
            }

            if (rating == 4)
            {
                if (index >= totalRooms - 2) return "King Suite";
                if (index >= totalRooms - 5) return "Family Room";
                if (index % 4 == 0) return "Deluxe Double";
                if (index % 3 == 0) return "Twin Room";
                return "Standard Double";
            }

            if (rating == 3)
            {
                if (index >= totalRooms - 2) return "Family Room";
                if (index % 4 == 0) return "Twin Room";
                if (index % 3 == 0) return "Single Room";
                return "Standard Double";
            }

            if (index % 4 == 0) return "Single Room";
            if (index % 3 == 0) return "Twin Room";
            return "Standard Double";
        }

        private static int GetCapacity(string roomType)
        {
            return roomType switch
            {
                "Single Room" => 1,
                "Standard Double" => 2,
                "Deluxe Double" => 2,
                "Twin Room" => 2,
                "Family Room" => 4,
                "Family Suite" => 5,
                "King Suite" => 4,
                "Executive Suite" => 4,
                "Presidential Suite" => 6,
                _ => 2
            };
        }

        private static decimal CalculateRoomPrice(decimal hotelBasePrice, string roomType)
        {
            decimal multiplier = roomType switch
            {
                "Single Room" => 0.55m,
                "Standard Double" => 0.75m,
                "Deluxe Double" => 0.95m,
                "Twin Room" => 0.80m,
                "Family Room" => 1.10m,
                "Family Suite" => 1.35m,
                "King Suite" => 1.45m,
                "Executive Suite" => 1.60m,
                "Presidential Suite" => 2.10m,
                _ => 0.80m
            };

            var calculated = Math.Round(hotelBasePrice * multiplier, 2);

            if (calculated < 1000)
                calculated = 1000;

            return calculated;
        }

        private static string GetDescription(string roomType)
        {
            return roomType switch
            {
                "Single Room" => "Comfortable room ideal for solo travelers with essential amenities.",
                "Standard Double" => "Modern double room with a cozy atmosphere and stylish interior.",
                "Deluxe Double" => "Spacious deluxe room with upgraded comfort and elegant design.",
                "Twin Room" => "Twin room with separate beds, perfect for friends or colleagues.",
                "Family Room" => "Large family room suitable for group stays and extra comfort.",
                "Family Suite" => "Premium family suite with extra living space and enhanced amenities.",
                "King Suite" => "Luxury suite with king bed, refined decor, and premium comfort.",
                "Executive Suite" => "Executive-level suite designed for business and premium stays.",
                "Presidential Suite" => "Top-tier suite offering the highest level of luxury and space.",
                _ => "Comfortable room with modern amenities."
            };
        }

        private static string GetBedType(string roomType)
        {
            return roomType switch
            {
                "Single Room" => "1 Single Bed",
                "Standard Double" => "1 Double Bed",
                "Deluxe Double" => "1 Queen Bed",
                "Twin Room" => "2 Single Beds",
                "Family Room" => "1 Double Bed + 2 Single Beds",
                "Family Suite" => "2 Double Beds",
                "King Suite" => "1 King Bed",
                "Executive Suite" => "1 King Bed",
                "Presidential Suite" => "2 King Beds",
                _ => "1 Double Bed"
            };
        }

        private static string GetViewType(int rating, int index)
        {
            if (rating >= 5 && index % 4 == 0) return "Sea View";
            if (rating >= 4 && index % 3 == 0) return "Pool View";
            if (index % 2 == 0) return "City View";
            return "Garden View";
        }

        private static bool HasBalcony(string roomType)
        {
            return roomType is "Deluxe Double" or "Family Suite" or "King Suite" or "Executive Suite" or "Presidential Suite";
        }

        private static bool HasBreakfast(int rating)
        {
            return rating >= 3;
        }
    }
}