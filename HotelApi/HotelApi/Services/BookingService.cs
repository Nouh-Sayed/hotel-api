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
            var bookings = await _context.Bookings.AsNoTracking().ToListAsync();
            return bookings.Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                CustomerId = b.CustomerId,
                HotelId = b.HotelId,
                RoomId = b.RoomId,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Adults = b.Adults,
                Children = b.Children,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            });
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            var booking = await BuildBookingAsync(
                dto.CustomerId, dto.HotelId, dto.RoomId,
                dto.CheckInDate, dto.CheckOutDate, dto.Adults, dto.Children);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Map(booking);
        }

        public async Task<BookingDto> CreateReservationAsync(CreateReservationDto dto)
        {
            var customer = new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Email = dto.Email,
                Nationality = dto.Nationality,
                IdentityNumber = dto.IdentityNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var booking = await BuildBookingAsync(
                customer.CustomerId, dto.HotelId, dto.RoomId,
                dto.CheckInDate, dto.CheckOutDate, dto.Adults, dto.Children);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Map(booking);
        }

        private async Task<Booking> BuildBookingAsync(
            int customerId, int hotelId, int roomId,
            DateTime checkInDate, DateTime checkOutDate,
            int adults, int children)
        {
            if (checkOutDate <= checkInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId && r.HotelId == hotelId);
            if (room == null)
                throw new InvalidOperationException("Room not found in this hotel.");

            if (!room.IsAvailable)
                throw new InvalidOperationException("Room is not available.");

            var isRoomBooked = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status != "Cancelled" &&
                checkInDate < b.CheckOutDate &&
                checkOutDate > b.CheckInDate
            );

            if (isRoomBooked)
                throw new InvalidOperationException("Room is already booked for this period.");

            var nights = (checkOutDate - checkInDate).Days;
            var totalPrice = nights * room.PricePerNight;

            return new Booking
            {
                CustomerId = customerId,
                HotelId = hotelId,
                RoomId = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                Adults = adults,
                Children = children,
                TotalPrice = totalPrice,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow
            };
        }

        private static BookingDto Map(Booking b) => new()
        {
            BookingId = b.BookingId,
            CustomerId = b.CustomerId,
            HotelId = b.HotelId,
            RoomId = b.RoomId,
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            Adults = b.Adults,
            Children = b.Children,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            CreatedAt = b.CreatedAt
        };
    }
}