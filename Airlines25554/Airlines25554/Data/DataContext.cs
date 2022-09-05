using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airlines25554.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
              public DbSet<AirPlane> AirPlanes { get; set; }    
    }
}

