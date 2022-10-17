using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMailHelper _mailHelper;
        private readonly ITicketPurchasedRepository _ticketPurchasedRepository;
        private readonly IPassengerRepository _passengerRepository;

        public BookingsController(
            IFlightRepository flightRepository,
            ICountryRepository countryRepository,
            ITicketRepository ticketRepository,
            UserManager<User> userManager,
            IMailHelper mailHelper,
            ITicketPurchasedRepository ticketPurchasedRepository,
            IPassengerRepository passengerRepository)
        {
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _mailHelper = mailHelper;
            _ticketPurchasedRepository = ticketPurchasedRepository;
            _passengerRepository = passengerRepository;
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


        public async Task<IActionResult> PassengerData(int? id)
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





        public IActionResult Booking(PassengerViewModel model)
        {

            // 1º: Obter a lista das classes
            var list = _flightRepository.GetComboClasses(); // Obter as classes

            // 3º: Verificar a existência do voo
            var flight = _flightRepository.GetFlight(model.FlightId);
            if (flight == null)
            {
                return this.RedirectToAction("Index", "Flights");
            }

            //4º: Obter a lista de bilhetes existentes para o voo

            var ticketsList = _ticketRepository.FlightTickets(model.FlightId);

            model.FlightId = flight.Id;
            model.Classes = list;
            model.FirstName = model.FirstName;
            model.LastName = model.LastName;
            model.PassportId = model.PassportId;
            

            model.TotalSeatsList = ticketsList.ToList();
            //model.TotalSeatsList = ticketsList.ToList();
            //model.TotalSeatsList = ticketsList.ToList();

         
            
            return View(model);

        }


        public async Task<IActionResult> ShowTicket(PassengerViewModel model)
        {
     

            var ticket = await _ticketRepository.GetByIdAsync(model.TicketId);


            if (ModelState.IsValid)
            {

                var flight = await _flightRepository.GetFlightWithObjectsAsync(model.FlightId);

                var destinationFrom = flight.From;
                var destinationTo = flight.To;

                var from = await _countryRepository.GetCityWithAirportAsync(destinationFrom);
                var to = await _countryRepository.GetCityWithAirportAsync(destinationTo);

                model.From  =  from.Name;
                model.To    =    to.Name;
                model.Date  =  flight.Departure.ToShortDateString();
                model.Time  =  flight.Departure.ToShortTimeString();
                model.Class = ticket.Class;
                model.Seat  =  ticket.Seat;

                return View(model);

            }

            return this.RedirectToAction("Index", "Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTicket(PassengerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User); // -> Devolve o user que está logado

                //var userId = loggedUser.Id; // id -> do user que está logado

                var flight = _flightRepository.GetFlight(model.FlightId);

                Passenger passenger = new Passenger()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PassportId = model.PassportId,
                    User = loggedUser,

                };

                TicketPurchased ticketPurchased = new TicketPurchased();

                ticketPurchased.User = loggedUser;
                ticketPurchased.Flight = flight;
                ticketPurchased.Seat = model.Seat;
                ticketPurchased.Class = model.Class;


                var ticket = _ticketRepository.GetTicketById(model.TicketId);

                try
                {
                    await _ticketPurchasedRepository.CreateAsync(ticketPurchased);// Ao usar o create grava logo
                    _ticketRepository.UpdateTicketIsAvailableAsync(ticket);
                    await _passengerRepository.CreateAsync(passenger);


                    _mailHelper.SendEmail(passenger.Email, "Ticket", $"<h1>Ticket Confirmation</h1>" +
                       $"Your ticket information, " +
                       $"Flight: {model.FlightId}, " +
                       $"Class: {ticket.Class}, " +
                       $"Date: {model.Date}, ");
                  


                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    return View(model);
                }

            }

            return this.RedirectToAction("Index", "Flights");
        }
    }
}

