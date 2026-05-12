using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] CreatePaymentDto dto)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            var payment = new Payment
            {
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Paid",
                TransactionCode = $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment completed successfully.",
                paymentId = payment.PaymentId,
                transactionCode = payment.TransactionCode
            });
        }
    }
}