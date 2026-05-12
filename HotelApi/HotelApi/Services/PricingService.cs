using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class PricingService : IPricingService
    {
        private readonly ApplicationDbContext _context;

        public PricingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PricingResultDto> CalculateAsync(PricingCalculationDto dto)
        {
            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId);

            if (room == null)
                throw new InvalidOperationException("Room not found.");

            var nights = (dto.CheckOutDate.Date - dto.CheckInDate.Date).Days;

            decimal multiplier = 1m;
            var appliedRules = new List<string>();

            var rules = await _context.PricingRules
                .Where(r =>
                    r.IsActive &&
                    dto.CheckInDate.Date <= r.EndDate.Date &&
                    dto.CheckOutDate.Date >= r.StartDate.Date &&
                    (r.HotelId == null || r.HotelId == room.HotelId))
                .ToListAsync();

            foreach (var rule in rules)
            {
                multiplier *= rule.Multiplier;
                appliedRules.Add($"{rule.Name} x{rule.Multiplier}");
            }

            for (var date = dto.CheckInDate.Date; date < dto.CheckOutDate.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    multiplier *= 1.10m;
                    appliedRules.Add($"Weekend {date:yyyy-MM-dd} x1.10");
                }
            }

            var finalPricePerNight = Math.Round(room.PricePerNight * multiplier, 2);
            var totalPrice = Math.Round(finalPricePerNight * nights, 2);

            return new PricingResultDto
            {
                RoomId = room.RoomId,
                BasePricePerNight = room.PricePerNight,
                Nights = nights,
                FinalPricePerNight = finalPricePerNight,
                TotalPrice = totalPrice,
                AppliedRules = appliedRules
            };
        }

        public async Task<IEnumerable<PricingRuleDto>> GetRulesAsync()
        {
            var rules = await _context.PricingRules
                .Include(r => r.Hotel)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return rules.Select(Map);
        }

        public async Task<PricingRuleDto> CreateRuleAsync(CreatePricingRuleDto dto)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("End date must be after start date.");

            if (dto.HotelId.HasValue)
            {
                var hotelExists = await _context.Hotels.AnyAsync(h => h.HotelId == dto.HotelId.Value);
                if (!hotelExists)
                    throw new InvalidOperationException("Hotel not found.");
            }

            var rule = new PricingRule
            {
                HotelId = dto.HotelId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Multiplier = dto.Multiplier,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            _context.PricingRules.Add(rule);
            await _context.SaveChangesAsync();

            var created = await _context.PricingRules
                .Include(r => r.Hotel)
                .FirstAsync(r => r.PricingRuleId == rule.PricingRuleId);

            return Map(created);
        }

        public async Task DeleteRuleAsync(int id)
        {
            var rule = await _context.PricingRules
                .FirstOrDefaultAsync(r => r.PricingRuleId == id);

            if (rule == null)
                throw new InvalidOperationException("Pricing rule not found.");

            _context.PricingRules.Remove(rule);
            await _context.SaveChangesAsync();
        }

        private static PricingRuleDto Map(PricingRule rule)
        {
            return new PricingRuleDto
            {
                PricingRuleId = rule.PricingRuleId,
                HotelId = rule.HotelId,
                HotelName = rule.Hotel?.Name ?? "All Hotels",
                Name = rule.Name,
                StartDate = rule.StartDate,
                EndDate = rule.EndDate,
                Multiplier = rule.Multiplier,
                Description = rule.Description,
                IsActive = rule.IsActive
            };
        }
    }
}