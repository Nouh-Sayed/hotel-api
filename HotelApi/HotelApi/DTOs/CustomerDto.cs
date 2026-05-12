namespace HotelApi.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string NationalIdOrPassport { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}