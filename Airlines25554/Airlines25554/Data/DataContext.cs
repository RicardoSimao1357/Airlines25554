using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Airlines25554.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<AirPlane> AirPlanes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }



        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Country>()
           .HasIndex(c => c.Name)
           .IsUnique();

            // State Unique
            modelBuilder.Entity<Status>()
                .HasIndex(b => b.StatusName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

