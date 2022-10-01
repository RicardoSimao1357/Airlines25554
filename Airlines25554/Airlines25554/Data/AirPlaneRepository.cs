using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public class AirPlaneRepository : GenericRepository<AirPlane>, IAirPlaneRepository
    {
        private readonly DataContext _context;

        public AirPlaneRepository(DataContext context) : base(context)  
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.AirPlanes.Include(p => p.User);
        }

        public async Task<AirPlane> GetAirplaneWithUserAsync(int id)
        {

            var airplaine = await _context.AirPlanes
                            .Include(d => d.User)
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

            return airplaine;
        }
    }
}
