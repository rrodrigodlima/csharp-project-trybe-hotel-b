using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 2. Desenvolva o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            // LINQ para obter todas as cidades e mapeá-las para CityDto
            var cities = _context.Cities
                .Select(city => new CityDto
                {
                    CityId = city.CityId,
                    Name = city.Name
                })
                .ToList();

            return cities;
        }

        // 3. Desenvolva o endpoint POST /city
        public CityDto AddCity(City city)
        {
            // Adiciona a nova cidade ao contexto
            _context.Cities.Add(city);
            _context.SaveChanges();

            // Mapeia a nova cidade para o formato CityDto e retorna >>
            var addedCityDto = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name
            };

            return addedCityDto;
        }

    }
}