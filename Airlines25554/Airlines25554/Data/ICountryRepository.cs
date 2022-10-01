using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        IQueryable GetCountriesWithCities(); // -> Devolve os paises com as respetivas 

        IQueryable GetCitiesWithAirports();

        Task<Country> GetCountryWithCitiesAsync(int id);

        Task<City> GetCityWithAirportsAsync(int id);

        Task<City> GetCityAsync(int id);

        Task<Airport> GetAirportAsync(int id);

        Task AddCityAsync(CityViewModel model);

        Task AddAirportAsync(AirportViewModel model);

        Task<int> UpdateCityAsync(City city);

        Task<int> UpdateAirportAsync(Airport airport);

        Task<int> DeleteCityAsync(City city);

        Task<int> DeleteAirportAsync(Airport airport);

        IEnumerable<SelectListItem> GetComboCountries();

        IEnumerable<SelectListItem> GetComboCities(int countryId);

         IEnumerable<SelectListItem> GetComboAirports(int cityId);

        Task<Country> GetCountryAsync(City city);

        Task<City> GetCityWithAirportAsync(Airport airport);
    }
}
