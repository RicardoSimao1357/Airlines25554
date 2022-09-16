using Airlines25554.Data.Entities;
using System.Linq;

namespace Airlines25554.Data
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public IQueryable GetAllWithUsers();
    }
}
