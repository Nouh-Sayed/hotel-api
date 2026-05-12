//using HotelApi.Data;
//using HotelApi.Models;
//using Microsoft.EntityFrameworkCore;

//namespace HotelApi.Services
//{
//    public class SmartPriceUpdaterService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly Random _random = new();

//        public SmartPriceUpdaterService(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            Console.WriteLine("SMART PRICE SERVICE STARTED");
//            // يبدأ بعد 10 ثواني من تشغيل السيرفر
//            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                await UpdatePrices(stoppingToken);

//                Console.WriteLine($"Prices updated at: {DateTime.Now}");

//                // كل دقيقة
//                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
//            }
//        }

//        private async Task UpdatePrices(CancellationToken stoppingToken)
//        {
//            using var scope = _scopeFactory.CreateScope();

//            var context = scope.ServiceProvider
//                .GetRequiredService<ApplicationDbContext>();

//            var rooms = await context.Rooms
//                .Include(r => r.Hotel)
//                .ToListAsync(stoppingToken);

//            foreach (var room in rooms)
//            {
//                // تغيير عشوائي بين -10% و +15%
//                room.PricePerNight += 100;

//                Console.WriteLine(
//                    $"Room {room.RoomId} new price => {room.PricePerNight} TRY"
//                );

//                Console.WriteLine(
//                    $"Room {room.RoomId} => {room.PricePerNight} TRY"
//                );
//            }

//            await context.SaveChangesAsync(stoppingToken);
//        }
//    }
//}





























using HotelApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class SmartPriceUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SmartPriceUpdaterService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("SMART PRICE SERVICE STARTED");

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdatePrices(stoppingToken);

                Console.WriteLine($"Smart prices updated at: {DateTime.Now}");

                await Task.Delay(TimeSpan.FromDays(14), stoppingToken);
            }
        }

        private async Task UpdatePrices(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var rooms = await context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Bookings)
                .ToListAsync(stoppingToken);

            var today = DateTime.Today;
            var next14Days = today.AddDays(14);

            foreach (var room in rooms)
            {
                var basePrice = room.PricePerNight;
                var multiplier = 1.00m;

                if (today.Month is 6 or 7 or 8)
                    multiplier += 0.25m;

                if (today.Month is 1 or 2)
                    multiplier -= 0.10m;

                if (today.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday)
                    multiplier += 0.15m;

                if (room.Hotel.Rating >= 5)
                    multiplier += 0.12m;

                var cityText = $"{room.Hotel.City} {room.Hotel.Address}".ToLower();

                if (
                    cityText.Contains("antalya") ||
                    cityText.Contains("bodrum") ||
                    cityText.Contains("alanya") ||
                    cityText.Contains("marmaris") ||
                    cityText.Contains("fethiye") ||
                    cityText.Contains("akbük") ||
                    cityText.Contains("akbuk")
                )
                {
                    multiplier += 0.18m;
                }

                var activeBookings = room.Bookings.Count(b =>
                    b.Status != "Cancelled" &&
                    b.CheckInDate < next14Days &&
                    b.CheckOutDate > today
                );

                if (activeBookings >= 3)
                    multiplier += 0.20m;
                else if (activeBookings == 0)
                    multiplier -= 0.08m;

                var newPrice = basePrice * multiplier;

                if (newPrice < 1000)
                    newPrice = 1000;

                if (newPrice > 50000)
                    newPrice = 50000;

                room.PricePerNight = Math.Round(newPrice, 2);
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}


