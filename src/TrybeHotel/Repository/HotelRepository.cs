using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels
             .Join(
                 _context.Cities,
                 hotel => hotel.CityId,
                 city => city.CityId,
                 (hotel, city) => new HotelDto
                 {
                     HotelId = hotel.HotelId,
                     Name = hotel.Name,
                     Address = hotel.Address,
                     CityId = city.CityId,
                     CityName = city.Name
                 })
             .ToList();

            return hotels;
        }

        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            // Adiciona o novo hotel ao contexto
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            // Consulta a cidade relacionada para obter o nome da cidade
            var city = _context.Cities.First(c => c.CityId == hotel.CityId);

            // Mapeia o novo hotel e a cidade relacionada para o formato HotelDto e retorna
            var addedHotelDto = new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = city.Name
            };

            return addedHotelDto;
        }
    }
}