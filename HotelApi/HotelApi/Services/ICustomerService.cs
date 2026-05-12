using HotelApi.DTOs;

namespace HotelApi.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int customerId);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto> UpdateAsync(int customerId, UpdateCustomerDto dto);
        Task DeleteAsync(int customerId);
    }
}