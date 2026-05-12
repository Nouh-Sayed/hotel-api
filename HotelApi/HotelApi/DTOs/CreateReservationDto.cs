namespace HotelApi.DTOs
{
    public class CreateReservationDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string NationalIdOrPassport { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public int HotelId { get; set; }
        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }

        public decimal TotalPrice { get; set; }
    }
}