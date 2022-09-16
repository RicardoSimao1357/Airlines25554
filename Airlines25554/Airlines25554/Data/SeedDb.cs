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

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Employee");


            // Verificar se o user já está criado ( vai procurar o user através do Username.
            var user = await _userHelper.GetUserByUserNameAsync("RicardoSimao");

            var user2 = await _userHelper.GetUserByUserNameAsync("Ricardo1357");

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

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin"); 
            }

            if (!_context.AirPlanes.Any())
            {
                AddAirplane(user);
                await _context.SaveChangesAsync(); // Insere os aviões na base de dados
            }


            if (user2 == null)
            {
                user2 = new User
                {
                    FirstName = "Ricardo",
                    LastName = "Alves",
                    Email = "ricardo.simao.25554@formandos.cinel.pt",
                    UserName = "Ricardo1357",
                    Address = "Xpto",

                };

                // Criar o user:
                var result2 = await _userHelper.AddUserAsync(user2, "123456");

                if (result2 != IdentityResult.Success) // Se algo não correu bem vou lançar uma excepção
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user2, "Customer");
            }

            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Customer");

            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Customer");
            }

            if (!_context.Customers.Any())
            {
                AddCustomer(user2);
                await _context.SaveChangesAsync(); // Insere os aviões na base de dados
            }

        }

        private void AddCustomer(User user2)
        {
            _context.Customers.Add(new Customer
            {
                FirstName = user2.FirstName,
                LastName = user2.LastName,
                Email = user2.Email,    
                Address = user2.Address,
                PassportId = "919191919",
                User = user2
            });
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
