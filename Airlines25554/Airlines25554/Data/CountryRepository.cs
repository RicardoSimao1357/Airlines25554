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
            var country = await this.GetCountryWithAirportsAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Airports.Add(new Airport { Name = model.Name });
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAirportAsync(Airport airport)
        {
            var country = await _context.Countries
                  .Where(a => a.Airports.Any(ap => ap.Id == airport.Id))
                  .FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<Airport> GetAirportAsync(int id)
        {
            return await _context.Airports.FindAsync(id);
        }

        public IEnumerable<SelectListItem> GetComboAirports(int countryId)
        {
            var country = _context.Countries.Find(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = _context.Airports.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()

                }).OrderBy(l => l.Text).ToList();


                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a airport...)",
                    Value = "0"
                });

            }

            return list;
        }

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

        public IQueryable GetCountriesWithAirports()
        {
            return _context.Countries
                .Include(a => a.Airports)
                .OrderBy(a => a.Name);
        }

        public async  Task<Country> GetCountryAsync(Airport airport)
        {
            return await _context.Countries
               .Where(a => a.Airports.Any(ap => ap.Id == airport.Id))
               .FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryWithAirportsAsync(int id)
        {
            return await _context.Countries
                .Include(a => a.Airports)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAirportAsync(Airport airport)
        {
            var country = await _context.Countries
               .Where(a => a.Airports.Any(ap => ap.Id == airport.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Airports.Update(airport);
            await _context.SaveChangesAsync();
            return country.Id;
        }
    }
}
