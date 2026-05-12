using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            return Ok(await _customerService.GetAllAsync());
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<CustomerDto>> GetById(int customerId)
        {
            var customer = await _customerService.GetByIdAsync(customerId);

            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var customer = await _customerService.CreateAsync(dto);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{customerId:int}")]
        public async Task<ActionResult<CustomerDto>> Update(int customerId, [FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var customer = await _customerService.UpdateAsync(customerId, dto);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{customerId:int}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                await _customerService.DeleteAsync(customerId);
                return Ok(new { message = "Customer deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}