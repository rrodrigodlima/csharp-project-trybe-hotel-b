using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId) ?? throw new Exception("Quarto não encontrado");

            User user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new Exception("Usuário não encontrado");

            int? lastId = _context.Bookings.OrderBy(b => b.BookingId).LastOrDefault()?.BookingId;
            int newId = (int)(lastId == null ? 1 : lastId + 1);

            Booking newBooking = new()
            {
                BookingId = newId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = user.UserId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            Hotel hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId) ?? throw new Exception("Hotel não encontrado");

            City city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId) ?? throw new Exception("Cidade não encontrada");

            return new BookingResponse
            {
                BookingId = newId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = newBooking.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name
                    }
                }
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            throw new NotImplementedException();
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

        public object GetUserByEmail(string? name)
        {
            throw new NotImplementedException();
        }
    }

}