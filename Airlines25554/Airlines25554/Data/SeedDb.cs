using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            // Adicionar os estados
            if (!_context.Status.Any())
            {

                _context.Status.Add(new Status { StatusName = "Active" });
                _context.Status.Add(new Status { StatusName = "Canceled" });
                _context.Status.Add(new Status { StatusName = "Concluded" });

                await _context.SaveChangesAsync();
            }

            if (!_context.Countries.Any())
            {
                var airports = new List<Airport>();
                var airports2 = new List<Airport>();
                var airports3 = new List<Airport>();
                var cities = new List<City>();
                var cities2 = new List<City>();
                var cities3 = new List<City>();

                airports.Add(new Airport
                {
                    Name = "Airport Humberto Delgado" 
                });
      
                
                airports2.Add(new Airport
                {
                    Name = "Airport Madrid-Barajas"
                });

                airports3.Add(new Airport
                {
                    Name = "Airport Paris-Charles de Gaulle"
                });


                cities.Add(new City
                {
                    Name = "Lisbon", Airports = airports 
                });
    

                cities2.Add(new City
                {
                    Name = "Madrid",
                    Airports = airports2
                });

                cities3.Add(new City
                {
                    Name = "Paris",
                    Airports = airports3
                });

                _context.Countries.Add(new Country
                {  
                    Name = "Portugal",
                    Cities = cities
                });

                _context.Countries.Add(new Country
                {
                    Name = "Spain",
                    Cities = cities2
                });


                _context.Countries.Add(new Country
                {
                    Name = "France",
                    Cities = cities3
                });
            }






            // Verificar se o user já está criado ( vai procurar o user através do Username.

            var user = await _userHelper.GetUserByUserNameAsync("RicardoSimao"); //  -> Admin

            var user2 = await _userHelper.GetUserByUserNameAsync("Ricardo1357");  // -> Customer

            var user3 = await _userHelper.GetUserByUserNameAsync("Ricardo25554"); // -> Employee



            if (user == null)
            {
                user = new User
                {
                    Email = "ricardo.simao.1357@gmail.com",
                    UserName = "RicardoSimao",

                };

                // Criar o user:
                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success) // Se algo não correu bem vou lançar uma excepção
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
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

            //__________________________________________ criar o Custumer_________________________________________//

            if (user2 == null)
            {
                user2 = new User
                {
                    Email = "ricardo.simao.25554@formandos.cinel.pt",
                    UserName = "Ricardo1357",
                };

                // Criar o user:
                var result2 = await _userHelper.AddUserAsync(user2, "123456");

                if (result2 != IdentityResult.Success) // Se algo não correu bem vou lançar uma excepção
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user2, "Customer");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user2);
                await _userHelper.ConfirmEmailAsync(user2, token);
            }

            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Customer");

            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Customer");
            }

            if (!_context.Customers.Any())
            {
                AddCustomer(user2);
                await _context.SaveChangesAsync(); // Insere os Customers na base de dados
            }

            //__________________________________________ criar o Employee_________________________________________//

            if (user3 == null)
            {
                user3 = new User
                {
                    Email = "ricardocodingtester@gmail.com",
                    UserName = "Ricardo25554",
                };

                // Criar o user:
                var result3 = await _userHelper.AddUserAsync(user3, "123456");

                if (result3 != IdentityResult.Success) // Se algo não correu bem vou lançar uma excepção
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user3, "Employee");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user3);
                await _userHelper.ConfirmEmailAsync(user3, token);
            }

            var isInRole3 = await _userHelper.IsUserInRoleAsync(user3, "Employee");

            if (!isInRole3)
            {
                await _userHelper.AddUserToRoleAsync(user3, "Employee");
            }

            if (!_context.Employees.Any())
            {
                AddEmployee(user3);
                await _context.SaveChangesAsync(); // Insere os Employees na base de dados
            }
        }

        private void AddEmployee(User user3)
        {
            _context.Employees.Add(new Employee
            {
                FirstName = "Ricardo",
                LastName = "António",
                Address = "Av.xpto",
                DocumentId = "919191919",
                User = user3
            });
        }

        private void AddCustomer(User user2)
        {
            _context.Customers.Add(new Customer
            {
                FirstName = "Ricardo",
                LastName = "Alves",
                Address = "Av.xpto",
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
