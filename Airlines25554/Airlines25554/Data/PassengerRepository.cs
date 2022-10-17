using Airlines25554.Data.Entities;

namespace Airlines25554.Data
{
    public class PassengerRepository : GenericRepository<Passenger>, IPassengerRepository
    {
        private readonly DataContext _context;

        public PassengerRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
