using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<PricingResultDto>> Calculate([FromBody] PricingCalculationDto dto)
        {
            try
            {
                var result = await _pricingService.CalculateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("rules")]
        public async Task<ActionResult<IEnumerable<PricingRuleDto>>> GetRules()
        {
            return Ok(await _pricingService.GetRulesAsync());
        }

        [HttpPost("rules")]
        public async Task<ActionResult<PricingRuleDto>> CreateRule([FromBody] CreatePricingRuleDto dto)
        {
            try
            {
                var result = await _pricingService.CreateRuleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("rules/{id:int}")]
        public async Task<IActionResult> DeleteRule(int id)
        {
            try
            {
                await _pricingService.DeleteRuleAsync(id);
                return Ok(new { message = "Pricing rule deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}