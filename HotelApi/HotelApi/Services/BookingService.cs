using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var bookings = await BaseQuery()
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(Map);
        }

        public async Task<BookingDto?> GetByIdAsync(int bookingId)
        {
            var booking = await BaseQuery()
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            return booking == null ? null : Map(booking);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            await ValidateBookingReferences(dto.CustomerId, dto.HotelId, dto.RoomId);

            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            var room = await _context.Rooms.FirstAsync(r => r.RoomId == dto.RoomId);

            if (!room.IsAvailable)
                throw new InvalidOperationException("Room is not available.");

            var booking = new Booking
            {
                CustomerId = dto.CustomerId,
                HotelId = dto.HotelId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Adults = dto.Adults,
                Children = dto.Children,
                TotalPrice = dto.TotalPrice,
                Status = dto.Status,
                SpecialRequests = dto.SpecialRequests ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);

            room.IsAvailable = false;

            await _context.SaveChangesAsync();

            var created = await BaseQuery()
                .FirstAsync(b => b.BookingId == booking.BookingId);

            return Map(created);
        }

        public async Task<BookingDto> UpdateAsync(int bookingId, UpdateBookingDto dto)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                throw new InvalidOperationException("Booking not found.");

            await ValidateBookingReferences(dto.CustomerId, dto.HotelId, dto.RoomId);

            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            if (booking.RoomId != dto.RoomId)
            {
                var oldRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == booking.RoomId);
                if (oldRoom != null)
                    oldRoom.IsAvailable = true;

                var newRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == dto.RoomId);
                if (newRoom == null)
                    throw new InvalidOperationException("Room not found.");

                if (!newRoom.IsAvailable)
                    throw new InvalidOperationException("New selected room is not available.");

                newRoom.IsAvailable = false;
            }

            booking.CustomerId = dto.CustomerId;
            booking.HotelId = dto.HotelId;
            booking.RoomId = dto.RoomId;
            booking.CheckInDate = dto.CheckInDate;
            booking.CheckOutDate = dto.CheckOutDate;
            booking.Adults = dto.Adults;
            booking.Children = dto.Children;
            booking.TotalPrice = dto.TotalPrice;
            booking.Status = dto.Status;
            booking.SpecialRequests = dto.SpecialRequests ?? string.Empty;

            await _context.SaveChangesAsync();

            var updated = await BaseQuery()
                .FirstAsync(b => b.BookingId == booking.BookingId);

            return Map(updated);
        }

        public async Task DeleteAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                throw new InvalidOperationException("Booking not found.");

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == booking.RoomId);

            if (room != null)
                room.IsAvailable = true;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Booking> BaseQuery()
        {
            return _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                .AsNoTracking();
        }

        private async Task ValidateBookingReferences(int customerId, int hotelId, int roomId)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
            if (!customerExists)
                throw new InvalidOperationException("Customer not found.");

            var hotelExists = await _context.Hotels.AnyAsync(h => h.HotelId == hotelId);
            if (!hotelExists)
                throw new InvalidOperationException("Hotel not found.");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null)
                throw new InvalidOperationException("Room not found.");

            if (room.HotelId != hotelId)
                throw new InvalidOperationException("Selected room does not belong to selected hotel.");
        }

        private static BookingDto Map(Booking b) => new()
        {
            BookingId = b.BookingId,
            CustomerId = b.CustomerId,
            CustomerName = b.Customer?.FullName ?? string.Empty,
            HotelId = b.HotelId,
            HotelName = b.Hotel?.Name ?? string.Empty,
            RoomId = b.RoomId,
            RoomNumber = b.Room?.RoomNumber ?? string.Empty,
            RoomType = b.Room?.RoomType ?? string.Empty,
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            Adults = b.Adults,
            Children = b.Children,
            TotalPrice = b.TotalPrice,
            Status = b.Status ?? string.Empty,
            SpecialRequests = b.SpecialRequests ?? string.Empty,
            CreatedAt = b.CreatedAt
        };
    }
}