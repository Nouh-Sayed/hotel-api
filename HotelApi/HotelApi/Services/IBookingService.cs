using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int bookingId);
        Task<BookingDto> CreateAsync(CreateBookingDto dto);
        Task<BookingDto> UpdateAsync(int bookingId, UpdateBookingDto dto);
        Task DeleteAsync(int bookingId);
    }
}