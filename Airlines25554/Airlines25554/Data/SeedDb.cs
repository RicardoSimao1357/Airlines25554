using System;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Airlines25554.Data
{
    public class SeedDb
    {
        private readonly DataContext _context; // -> privado e readonly porque é apenas a ligação á base de dados

        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;   
        }

        public async Task SeedAsync()
        {
            // Se a base de dados estiver criada nenhuma acção é tomada. Caso não exista, a base de dados é criada.
            await _context.Database.EnsureCreatedAsync();

            // Verificar se o user já está criado ( vai procurar o user através do email. O email será utilizado para a autenticação
            var user = await _userHelper.GetUserByUserNameAsync("RicardoSimao");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Ricardo",
                    LastName = "Simão",
                    Email = "ricardo.simao.1357@gmail.com",
                    UserName = "RicardoSimao",
                    Address = "Xpto",
              
                };

                // Criar o user:
                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success) // Se algo não correu bem vou lançar uma excepção
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if (!_context.AirPlanes.Any())
            {
                AddAirplane(user);
                await _context.SaveChangesAsync(); // Insere os aviões na base de dados
            }
        }

        private void AddAirplane(User user) // Verifica se a tabela de aviões tem algum avião, caso não tenha são inseridos aviões
        {
            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "xx-22-ff",
                AirplaneModel = "DC-6",
                EconomySeats = 56,
                ExecutiveSeats = 8,
                FirstClassSeats = 4,
                User = user

            });

            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "rr-22-ff",
                AirplaneModel = "Airbus A300",
                EconomySeats = 99,
                ExecutiveSeats = 5,
                FirstClassSeats = 4,
                User = user

            });


            _context.AirPlanes.Add(new AirPlane
            {
                Registration = "dd-11-ff",
                AirplaneModel = "Boeing 707",
                EconomySeats = 99,
                ExecutiveSeats = 11,
                FirstClassSeats = 4,
                User = user
            });
        }
    }
}
