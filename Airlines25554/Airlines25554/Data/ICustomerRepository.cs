using Airlines25554.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public IQueryable GetAllWithUsers();

        Task<User> GetUserByNameAsync(string username);

        Task<Customer> GetCustomerByUserAsync(User user);


    }
}
