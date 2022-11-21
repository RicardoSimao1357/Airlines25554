using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Customers.Include(p => p.User);
        }

        public async Task<Customer> GetCustomerByUserAsync(User user)
        {
            return await _context.Customers.Where(c => c.User == user)
          .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _context.Users.Where(u => u.UserName == username)
              .FirstOrDefaultAsync();
        }
    }
}
