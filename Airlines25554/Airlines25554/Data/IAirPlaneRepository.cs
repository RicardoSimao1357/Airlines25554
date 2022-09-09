using Airlines25554.Data.Entities;
using System.Linq;

namespace Airlines25554.Data
{
    public interface IAirPlaneRepository : IGenericRepository<AirPlane> 
    {
        public IQueryable GetAllWithUsers();
    }
}
