using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable GetCountriesWithAirports()
        {
            return _context.Countries
                .Include(a => a.Airports)
                .OrderBy(a => a.Name);
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
