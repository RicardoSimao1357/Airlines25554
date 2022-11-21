using Airlines25554.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();

        Employee GetEmployeeByIdAsync(string id);

        Task<Employee> GetEmployeeByUserAsync(User user);


    }
}
