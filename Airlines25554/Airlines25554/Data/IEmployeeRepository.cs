using Airlines25554.Data.Entities;
using System.Linq;

namespace Airlines25554.Data
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();
    }
}
