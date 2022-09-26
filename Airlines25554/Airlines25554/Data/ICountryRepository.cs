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
        IQueryable GetCountriesWithAirports();

        Task<Country> GetCountryWithAirportsAsync(int id);

        Task<Airport> GetAirportAsync(int id);

        Task AddAirportAsync(AirportViewModel model);

        Task<int> UpdateAirportAsync(Airport airport);

        Task<int> DeleteAirportAsync(Airport airport);

        IEnumerable<SelectListItem> GetComboCountries();

        IEnumerable<SelectListItem> GetComboAirports(int countryId);

        Task<Country> GetCountryAsync(Airport airport);
    }
}
