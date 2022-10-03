using Airlines25554.Data;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketRepository _ticketRepository;

        public BookingsController(
            IFlightRepository flightRepository,
            ICountryRepository countryRepository,
            ITicketRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<IActionResult> Index()
        {

            var model = new FlightViewModel()
            {
                States = _flightRepository.GetComboStatus(), // Apresentar uma combobox com os estado dos voos para posteriormente apresentar os voos na tabela
                Flights = _flightRepository.GetAllWithObjects(),

            };

            foreach (var item in model.Flights)
            {


                var destinationFrom = await _countryRepository.GetCityWithAirportAsync(item.From);

                var destinationTo = await _countryRepository.GetCityWithAirportAsync(item.To);

                item.From.Name = destinationFrom.Name.ToString(); // -> Recebo um Aeroporto e vou buscar a respetiva cidade 
                                                                  // -> Ao criar os Voos a origem e destino são aeroportos, mas assim que criados o cliente quando for á lista de voos
                                                                  // ve a cidade de destino e origem, apenas para efeito estético
                                                                  //
                item.To.Name = destinationTo.Name.ToString();     // -> Recebo um Aeroporto e vou buscar a respetiva cidade 
            }

            return View(model);
        }

        
        public async Task <IActionResult> PassengerData(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value);

            if (flight == null)
            {
                return NotFound();
            }
            ViewBag.flightId = flight.Id;


            if (flight.Status.StatusName == "Canceled" || flight.Status.StatusName == "Concluded")
            {
                ViewBag.Message = "The flight isn't active! It´s impossible to create tickets!";
                return View();
            }

            PassengerViewModel model = new PassengerViewModel();

            model.FlightId = flight.Id; 


            return View(model);  
        }


        public async Task<IActionResult> Booking(PassengerViewModel model)
        {

            // 1º: Obter a lista das classes
            var list = _flightRepository.GetComboClasses(); // Obter as classes

            // 2º: Verificar a existência do user
            //var user = await _userHelper.GetUserByEmailAsync(model.UserEmail);
            //if (user == null)
            //{
            //    return this.RedirectToAction("Index", "Flights");
            //}

            // 3º: Verificar a existência do voo
            var flight = _flightRepository.GetFlight(model.FlightId);
            if (flight == null)
            {
                return this.RedirectToAction("Index", "Flights");
            }

            //4º: Obter a lista de bilhetes existentes para o voo
            var ticketsList = _ticketRepository.FlightTickets(model.FlightId);


            // 4º Criar a lista com as classes para passar para a view
            var totalSeats = flight.BusyEconomicSeats + flight.BusyExecutiveSeats + flight.BusyFirstClassSeats;

            var economicSeats = flight.BusyEconomicSeats;

            var executiveSeats = flight.BusyExecutiveSeats;

            var firstClassSeats = flight.BusyFirstClassSeats;



            model.EconomicSeats = economicSeats;
            model.ExecutiveSeats = executiveSeats;
            model.FirstClassSeats = firstClassSeats;

            
            model.Email = model.Email;
            model.FlightId = flight.Id;
            model.Classes = list;
       
            return View(model);


        }

    }
}
