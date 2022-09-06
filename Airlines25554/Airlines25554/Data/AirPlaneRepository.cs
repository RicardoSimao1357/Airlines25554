using Airlines25554.Data.Entities;

namespace Airlines25554.Data
{
    public class AirPlaneRepository : GenericRepository<AirPlane>, IAirPlaneRepository
    {
        public AirPlaneRepository(DataContext context) : base(context)  
        {

        }
    }
}
