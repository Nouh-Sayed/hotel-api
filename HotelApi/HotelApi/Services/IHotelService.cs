using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface IHotelService
    {
        Task<PagedResultDto<HotelDto>> GetAllAsync(HotelQueryDto query);
        Task<HotelDto?> GetByExternalIdAsync(int externalId);
    }
}