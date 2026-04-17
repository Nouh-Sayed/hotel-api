using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<HotelDto>>> GetAll([FromQuery] HotelQueryDto query)
        {
            var result = await _hotelService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{externalId:int}")]
        public async Task<ActionResult<HotelDto>> GetById(int externalId)
        {
            var hotel = await _hotelService.GetByExternalIdAsync(externalId);

            if (hotel == null)
                return NotFound(new { message = "Hotel not found." });

            return Ok(hotel);
        }
    }
}