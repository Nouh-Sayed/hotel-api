using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    }
}