using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddAirportAsync(AirportViewModel model)
        {
            var city = await this.GetCityWithAirportsAsync(model.CityId);
            if (city == null)
            {
                return;
            }

            city.Airports.Add(new Airport { Name = model.Name });
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }

        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Cities.Add(new City { Name = model.Name });
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAirportAsync(Airport airport)
        {
            var city = await _context.Cities
               .Where(c => c.Airports.Any(ci => ci.Id == airport.Id))
               .FirstOrDefaultAsync();
            if (city == null)
            {
                return 0;
            }

            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
            return city.Id;
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await _context.Countries
                 .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                 .FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<Airport> GetAirportAsync(int id)
        {
            return await _context.Airports.FindAsync(id);
        }

        public IQueryable GetCitiesWithAirports()
        {
            return _context.Cities
               .Include(c => c.Airports)
               .OrderBy(c => c.Name);
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

        // -> Recebe um aeroporto e vai á tabela das cidades procurar a cidade respetiva
        public async Task<City> GetCityWithAirportAsync(Airport airport)
        {
            return await _context.Cities
             .Where(c => c.Airports.Any(ci => ci.Id == airport.Id))
             .FirstOrDefaultAsync();
        }

        public async Task<City> GetCityWithAirportsAsync(int id)
        {
            return await _context.Cities
               .Include(c => c.Airports)
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync();
        }

        // -> Preenche a combobox dos Airports
        public IEnumerable<SelectListItem> GetComboAirports(int cityId)
        {
            var city = _context.Cities.Find(cityId);
            var list = new List<SelectListItem>();
            if (city != null)
            {
                list = city.Airports.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a airport...)",
                Value = "0"
            });

            return list;
        }

        // -> Preenche a combobox das cities
        public IEnumerable<SelectListItem> GetComboCities(int countryId)
        {
            var country = _context.Countries.Find(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = country.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;
        }
            
        // -> Preenche a combobox dos Countries
        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)",
                Value = "0"
            });

            return list;
        }

        public IQueryable GetCountriesWithCities()
        {
            return _context.Countries
               .Include(c => c.Cities)
               .OrderBy(c => c.Name);
        }

        // -> Recebe uma cidade e vai á tabela dos paises procurar o pais respetivo
        public async Task<Country> GetCountryAsync(City city)
        {
            return await _context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))   
                .FirstOrDefaultAsync(); 
        }

        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await _context.Countries
               .Include(c => c.Cities)
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAirportAsync(Airport airport)
        {
            var city = await _context.Cities
             .Where(c => c.Airports.Any(ci => ci.Id == airport.Id)).FirstOrDefaultAsync();
            if (city == null)
            {
                return 0;
            }

            _context.Airports.Update(airport);
            await _context.SaveChangesAsync();
            return city.Id;
        }

        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await _context.Countries
               .Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }
    }
}
