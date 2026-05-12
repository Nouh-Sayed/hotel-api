using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _context.Rooms.AsNoTracking().ToListAsync();
            return rooms.Select(Map);
        }

        public async Task<IEnumerable<RoomDto>> GetByHotelAsync(int hotelId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();

            return rooms.Select(Map);
        }

        public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
        {
            var hotelExists = await _context.Hotels.AnyAsync(h => h.HotelId == dto.HotelId);
            if (!hotelExists)
                throw new InvalidOperationException("Hotel not found.");

            if (await _context.Rooms.AnyAsync(r => r.HotelId == dto.HotelId && r.RoomNumber == dto.RoomNumber))
                throw new InvalidOperationException("Room number already exists in this hotel.");

            var room = new Room
            {
                HotelId = dto.HotelId,
                RoomNumber = dto.RoomNumber,
                RoomType = dto.RoomType,
                Capacity = dto.Capacity,
                PricePerNight = dto.PricePerNight,
                IsAvailable = dto.IsAvailable,
                Description = dto.Description,
                BedType = dto.BedType,
                ViewType = dto.ViewType,
                HasBalcony = dto.HasBalcony,
                HasBreakfast = dto.HasBreakfast,
                HasWifi = dto.HasWifi,
                HasAirConditioning = dto.HasAirConditioning,
                HasTV = dto.HasTV
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return Map(room);
        }

        private static RoomDto Map(Room r) => new()
        {
            RoomId = r.RoomId,
            HotelId = r.HotelId,
            RoomNumber = r.RoomNumber,
            RoomType = r.RoomType,
            Capacity = r.Capacity,
            PricePerNight = r.PricePerNight,
            IsAvailable = r.IsAvailable,
            Description = r.Description,
            BedType = r.BedType,
            ViewType = r.ViewType,
            HasBalcony = r.HasBalcony,
            HasBreakfast = r.HasBreakfast,
            HasWifi = r.HasWifi,
            HasAirConditioning = r.HasAirConditioning,
            HasTV = r.HasTV
        };
    }
}