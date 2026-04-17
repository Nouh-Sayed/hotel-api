using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto> CreateBookingAsync(CreateBookingDto dto);
        Task<BookingDto> CreateReservationAsync(CreateReservationDto dto);
    }
}