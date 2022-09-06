using Airlines25554.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public class SeedDb
    {
        private readonly DataContext _context; // -> privado e readonly porque é apenas a ligação á base de dados

        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;

            _random = new Random();
        }

        public async Task SeedAsync()
        {
            // Se a base de dados estiver criada nenhuma acção é tomada. Caso não exista, a base de dados é criada.
            await _context.Database.EnsureCreatedAsync();

            if (!_context.AirPlanes.Any())
            {
                AddAirplane();
                await _context.SaveChangesAsync(); // Insere os aviões na base de dados
            }
        }

        private void AddAirplane() // Verifica se a tabela de aviões tem algum avião, caso não tenha são inseridos aviões
        {
            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "xx-22-ff",
                Model = "DC-6",
                EconomySeats = 56,
                ExecutiveSeats = 8,
                FirstClassSeats = 4
             //   User = user,

            });

            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "rr-22-ff",
                Model = "Airbus A300",
                EconomySeats = 99,
                ExecutiveSeats = 5,
                FirstClassSeats = 4
                //  User = user,

            });


            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "dd-11-ff",
                Model = "Boeing 707",
                EconomySeats = 99,
                ExecutiveSeats = 11,
                FirstClassSeats = 4
                //   User = user,
            });
        }
    }
}
