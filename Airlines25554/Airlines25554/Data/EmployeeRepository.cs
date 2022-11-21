using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Employees.Include(p => p.User);
        }

        public Employee GetEmployeeByIdAsync(string id)
        {
            return _context.Employees
                .Include(p => p.User)
                .FirstOrDefault(e => e.User.Id == id);
        }

        public async Task<Employee> GetEmployeeByUserAsync(User user)
        {
            return await _context.Employees.Where(c => c.User == user)
          .FirstOrDefaultAsync();
        }



    }
}
