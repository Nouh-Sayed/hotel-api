using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _context.Customers.AsNoTracking().ToListAsync();
            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                Nationality = c.Nationality,
                IdentityNumber = c.IdentityNumber
            });
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Email = dto.Email,
                Nationality = dto.Nationality,
                IdentityNumber = dto.IdentityNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Nationality = customer.Nationality,
                IdentityNumber = customer.IdentityNumber
            };
        }
    }
}