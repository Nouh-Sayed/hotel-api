using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ApplicationDbContext _context;

        public BookingsController(
            IBookingService bookingService,
            ApplicationDbContext context)
        {
            _bookingService = bookingService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            return Ok(await _bookingService.GetAllAsync());
        }

        [HttpGet("{bookingId:int}")]
        public async Task<ActionResult<BookingDto>> GetById(int bookingId)
        {
            var booking = await _bookingService.GetByIdAsync(bookingId);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var booking = await _bookingService.CreateAsync(dto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reservation")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            if (dto.CheckOutDate <= dto.CheckInDate)
                return BadRequest(new { message = "Check-out date must be after check-in date." });

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId && r.HotelId == dto.HotelId);

            if (room == null)
                return BadRequest(new { message = "Room not found for this hotel." });

            var blockedStatuses = new[] { "Pending", "Confirmed", "CheckedIn" };

            var hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == dto.RoomId &&
                blockedStatuses.Contains(b.Status) &&
                b.CheckInDate < dto.CheckOutDate &&
                b.CheckOutDate > dto.CheckInDate
            );

            if (hasOverlap)
                return BadRequest(new { message = "Room is not available for selected dates." });

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Nationality = dto.Nationality,
                    NationalIdOrPassport = dto.NationalIdOrPassport,
                  
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var booking = new Booking
            {
                CustomerId = customer.CustomerId,
                HotelId = dto.HotelId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Adults = dto.Adults,
                Children = dto.Children,
                TotalPrice = dto.TotalPrice,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            room.IsAvailable = false;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Booking created successfully.",
                bookingId = booking.BookingId,
                status = booking.Status
            });
        }

        [HttpPut("{bookingId:int}")]
        public async Task<ActionResult<BookingDto>> Update(int bookingId, [FromBody] UpdateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var booking = await _bookingService.UpdateAsync(bookingId, dto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{bookingId:int}")]
        public async Task<IActionResult> Delete(int bookingId)
        {
            try
            {
                await _bookingService.DeleteAsync(bookingId);
                return Ok(new { message = "Booking deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}