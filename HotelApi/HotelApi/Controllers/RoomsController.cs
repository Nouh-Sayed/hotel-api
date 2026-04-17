using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
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