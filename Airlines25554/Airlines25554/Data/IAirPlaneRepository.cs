using Airlines25554.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface IAirPlaneRepository : IGenericRepository<AirPlane> 
    {
        public IQueryable GetAllWithUsers();

        Task<AirPlane> GetAirplaneWithUserAsync(int id);
    }
}
