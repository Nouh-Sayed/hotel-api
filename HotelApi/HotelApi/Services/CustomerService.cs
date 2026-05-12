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
            var customers = await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.FullName)
                .ToListAsync();

            return customers.Select(Map);
        }

        public async Task<CustomerDto?> GetByIdAsync(int customerId)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            return customer == null ? null : Map(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var exists = await _context.Customers
                .AnyAsync(x => x.Email.ToLower() == email);

            if (exists)
                throw new InvalidOperationException("Email already exists.");

            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Nationality = dto.Nationality,
                IdentityNumber = dto.IdentityNumber,
                NationalIdOrPassport = dto.NationalIdOrPassport,
                Address = dto.Address
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Map(customer);
        }

        public async Task<CustomerDto> UpdateAsync(int customerId, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            var email = dto.Email.Trim().ToLower();

            var duplicateEmail = await _context.Customers.AnyAsync(c =>
                c.CustomerId != customerId &&
                c.Email.ToLower() == email);

            if (duplicateEmail)
                throw new InvalidOperationException("Email already exists.");

            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Nationality = dto.Nationality;
            customer.IdentityNumber = dto.IdentityNumber;
            customer.NationalIdOrPassport = dto.NationalIdOrPassport;
            customer.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Map(customer);
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        private static CustomerDto Map(Customer c) => new CustomerDto
        {
            CustomerId = c.CustomerId,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone,
            Nationality = c.Nationality,
            IdentityNumber = c.IdentityNumber,
            NationalIdOrPassport = c.NationalIdOrPassport,
            Address = c.Address
        };
    }
}