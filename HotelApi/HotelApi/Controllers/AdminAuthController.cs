using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/admin-auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto dto)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == dto.Username && a.IsActive);

            if (admin == null)
                return Unauthorized(new { message = "Invalid username or password." });

            var validPassword = PasswordHasher.Verify(dto.Password, admin.PasswordHash);

            if (!validPassword)
                return Unauthorized(new { message = "Invalid username or password." });

            return Ok(new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                FullName = admin.FullName,
                Role = admin.Role
            });
        }
    }
}