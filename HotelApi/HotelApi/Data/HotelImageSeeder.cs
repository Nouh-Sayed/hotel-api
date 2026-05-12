using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Data
{
    public static class HotelImageSeeder
    {
        private static readonly string[] BeachImages =
        {
            "https://images.unsplash.com/photo-1582719508461-905c673771fd?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1571896349842-33c89424de2d?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1571003123894-1f0594d2b5d9?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1519046904884-53103b34b206?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?q=80&w=1200&auto=format&fit=crop"
        };

        private static readonly string[] CityImages =
        {
            "https://images.unsplash.com/photo-1566073771259-6a8506099945?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1564501049412-61c2a3083791?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1444201983204-c43cbd584d93?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1455587734955-081b22074882?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?q=80&w=1200&auto=format&fit=crop"
        };

        private static readonly string[] NatureImages =
        {
            "https://images.unsplash.com/photo-1445019980597-93fa8acb246c?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1469474968028-56623f02e42e?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1506744038136-46273834b3fb?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1470770841072-f978cf4d019e?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1501785888041-af3ef285b470?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1439066615861-d1af74d74000?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?q=80&w=1200&auto=format&fit=crop"
        };

        private static readonly string[] LuxuryImages =
        {
            "https://images.unsplash.com/photo-1590490360182-c33d57733427?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1584132967334-10e028bd69f7?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1618773928121-c32242e63f39?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1522798514-97ceb8c4f1c8?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1584132915807-fd1f5fbc078f?q=80&w=1200&auto=format&fit=crop",
            "https://images.unsplash.com/photo-1578774204375-826dc5d996ed?q=80&w=1200&auto=format&fit=crop"
        };

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var hotels = await context.Hotels
                .Include(h => h.Images)
                .ToListAsync();

            foreach (var hotel in hotels)
            {
                if (hotel.Images.Any())
                {
                    context.HotelImages.RemoveRange(hotel.Images);
                }

                var images = PickImages(hotel);

                foreach (var imageUrl in images)
                {
                    context.HotelImages.Add(new HotelImage
                    {
                        HotelId = hotel.HotelId,
                        ImageUrl = imageUrl
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        private static IEnumerable<string> PickImages(Hotel hotel)
        {
            var text = $"{hotel.Name} {hotel.City} {hotel.Address}".ToLower();

            string[] pool;

            if (
                text.Contains("beach") ||
                text.Contains("sahil") ||
                text.Contains("bodrum") ||
                text.Contains("antalya") ||
                text.Contains("alanya") ||
                text.Contains("marmaris") ||
                text.Contains("fethiye") ||
                text.Contains("akbük") ||
                text.Contains("akbuk") ||
                text.Contains("adrasan") ||
                text.Contains("akyaka")
            )
            {
                pool = BeachImages;
            }
            else if (
                text.Contains("abant") ||
                text.Contains("göl") ||
                text.Contains("gol") ||
                text.Contains("dağ") ||
                text.Contains("dag") ||
                text.Contains("yayla") ||
                text.Contains("bolu") ||
                text.Contains("sapanca") ||
                text.Contains("nemrut") ||
                text.Contains("agva")
            )
            {
                pool = NatureImages;
            }
            else if (
                text.Contains("thermal") ||
                text.Contains("spa") ||
                text.Contains("wellness") ||
                text.Contains("resort") ||
                text.Contains("palace") ||
                hotel.Rating >= 5
            )
            {
                pool = LuxuryImages;
            }
            else
            {
                pool = CityImages;
            }

            return pool
                .OrderBy(x => Math.Abs(HashCode.Combine(
                    x,
                    hotel.HotelId,
                    hotel.ExternalId,
                    hotel.Name,
                    hotel.City
                )))
                .Take(4)
                .ToList();
        }
    }
}