using Airlines25554.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface IGenericRepository<T> where T : class 
    {
        IQueryable<T> GetAll(); // Método que devolve todas as entidades que o T estiver a usar


    

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }
}
