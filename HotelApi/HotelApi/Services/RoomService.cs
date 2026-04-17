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
            return rooms.Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                HotelId = r.HotelId,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                IsAvailable = r.IsAvailable
            });
        }

        public async Task<IEnumerable<RoomDto>> GetByHotelAsync(int hotelId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();

            return rooms.Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                HotelId = r.HotelId,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                IsAvailable = r.IsAvailable
            });
        }

        public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
        {
            var hotelExists = await _context.Hotels.AnyAsync(h => h.HotelId == dto.HotelId);
            if (!hotelExists)
                throw new InvalidOperationException("Hotel not found.");

            var room = new Room
            {
                HotelId = dto.HotelId,
                RoomNumber = dto.RoomNumber,
                RoomType = dto.RoomType,
                Capacity = dto.Capacity,
                PricePerNight = dto.PricePerNight,
                IsAvailable = dto.IsAvailable
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return new RoomDto
            {
                RoomId = room.RoomId,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Capacity = room.Capacity,
                PricePerNight = room.PricePerNight,
                IsAvailable = room.IsAvailable
            };
        }
    }
}