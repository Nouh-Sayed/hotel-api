using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ApplicationDbContext _context;

        public RoomsController(IRoomService roomService, ApplicationDbContext context)
        {
            _roomService = roomService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
        {
            return Ok(await _roomService.GetAllAsync());
        }

        [HttpGet("byHotel/{hotelId:int}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetByHotel(int hotelId)
        {
            return Ok(await _roomService.GetByHotelAsync(hotelId));
        }

        [HttpGet("available/byHotel/{hotelId:int}")]
        public async Task<IActionResult> GetAvailableRooms(
            int hotelId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            if (checkOut <= checkIn)
                return BadRequest(new { message = "Check-out must be after check-in." });

            var blockedStatuses = new[] { "Pending", "Confirmed", "CheckedIn" };

            var rooms = await _context.Rooms
                .Include(r => r.Bookings)
                .Where(r => r.HotelId == hotelId)
                .Where(r => !r.Bookings.Any(b =>
                    blockedStatuses.Contains(b.Status) &&
                    b.CheckInDate < checkOut &&
                    b.CheckOutDate > checkIn
                ))
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    HotelId = r.HotelId,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    Capacity = r.Capacity,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable,
                    Description = r.Description
                })
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var room = await _roomService.CreateAsync(dto);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}