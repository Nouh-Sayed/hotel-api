using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<IEnumerable<RoomDto>> GetByHotelAsync(int hotelId);
        Task<RoomDto> CreateAsync(CreateRoomDto dto);
    }
}