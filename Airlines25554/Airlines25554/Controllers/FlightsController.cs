using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airlines25554.Controllers
{
    public class FlightsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirPlaneRepository _airPlaneRepository;
        private readonly IMailHelper _mailHelper;
        private readonly ITicketRepository _ticketRepository;
        private Random _random; 

        public FlightsController(
            DataContext context,
            ICountryRepository countryRepository,
            IFlightRepository flightRepository,
            IAirPlaneRepository airPlaneRepository,
            IMailHelper mailHelper,
            ITicketRepository ticketRepository)
        {
            _context = context;
            _countryRepository = countryRepository;
            _flightRepository = flightRepository;
            _airPlaneRepository = airPlaneRepository;
            _mailHelper = mailHelper;
            _ticketRepository = ticketRepository;
            _flightRepository.UpdateFlightStatus(DateTime.Now);

            _random = new Random();
        }

        public IActionResult Index()
        {

            var model = new FlightViewModel()
            {
                States = _flightRepository.GetComboStatus(), // Apresentar uma combobox com os estado dos voos para posteriormente apresentar os voos na tabela
                Flights = _flightRepository.GetAllWithObjects(),

            };

  
            return View(model);

        }

        public IActionResult CreateFlight()
        {
            CreateFlightViewModel model = new CreateFlightViewModel
            {
                Airports = _flightRepository.GetComboAirports(),
                Airplanes = _flightRepository.GetComboAirplanes(),
                Status = _flightRepository.GetComboStatus(),
                StatusId = 1,
            };

            ViewBag.minDate = DateTime.Now;
            ViewBag.format = "dd/MM/yyyy HH:mm";


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight(CreateFlightViewModel model)
        {


            if (ModelState.IsValid)
            {

                var airplane = await _airPlaneRepository.GetAirplaneByName(model.Airplane);

                if (airplane == null)
                {
                    return NotFound();
                }

                // Verificar a disponibilidade do avião ( É visto na tebela dos voos - Enviando o id do avião)
                bool isAvailable = _flightRepository.AirplaneIsAvailable(airplane.Id, model.Departure, model.Arrival);

                // Obter o state active
                var status = _context.Status.Where(x => x.StatusName == "Active").FirstOrDefault();

                var to = await _flightRepository.GetAirportByName(model.To);
                var from = await _flightRepository.GetAirportByName(model.From);

                if (to == from)
                {
                    ModelState.AddModelError("", "The destinations must be diferent!");
                    return View(model);
                }



                if (isAvailable)
                {
                    Flight flight = new Flight()
                    {
                        From = from,
                        To = to,
                        Departure = model.Departure,
                        Arrival = model.Arrival,
                        AirPlane = airplane,
                        Status = status,
                        FlightNumber = model.FlightNumber,
                        BusyEconomicSeats = airplane.EconomySeats,
                        //BusyExecutiveSeats = airplane.ExecutiveSeats,
                        BusyFirstClassSeats = airplane.FirstClassSeats,

                    };

                    var economicSeats = flight.BusyEconomicSeats;
                    var firstClassSeats = flight.BusyFirstClassSeats;


                    try
                    {

                        await _flightRepository.CreateAsync(flight);

                        for (int i = 0; i < economicSeats; i++)
                        {

                            Ticket economicTickets = new Ticket()
                            {
                                Seat = (i + 1) + "ECO",
                                Class = "Economic",
                                Flight = flight,
                                IsAvailable = true,
                            };


                            await _ticketRepository.CreateAsync(economicTickets);
                        }

                        for (int i = 0; i < firstClassSeats; i++)
                        {

                            Ticket firstClassTickets = new Ticket()
                            {
                                Seat = (i + 1) + "FIRST",
                                Class = "First Class",
                                Flight = flight,
                                IsAvailable = true,
                            };


                            await _ticketRepository.CreateAsync(firstClassTickets);
                        }

                        return RedirectToAction(nameof(Index));
                    }

                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        GetCombos(model);
                        ViewBag.minDate = DateTime.Now;
                        ViewBag.format = "dd/MM/yyyy HH:mm";
                        ViewBag.departure = model.Departure;
                        ViewBag.arrival = model.Arrival;

                        return View(model);
                    }
                }

                GetCombos(model);
                ModelState.AddModelError(string.Empty, "Airplane isn't available. Choose another!");
                return View(model);


            }
            GetCombos(model);
            ViewBag.minDate = DateTime.Now;
            ViewBag.format = "dd/MM/yyyy HH:mm";
            return View(model);
        }


        // GET: Flight/Edit/5       
        public async Task<IActionResult> EditFlight(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value);
            //var flight = await _flightRepository.GetByIdAsync(id.Value);

            if (flight == null)
            {
                return NotFound();
            }

            CreateFlightViewModel model = new CreateFlightViewModel()
            {
                Id = flight.Id,
                Airplanes = _flightRepository.GetComboAirplanes(),
                Airports = _flightRepository.GetComboAirports(),
                Status = _flightRepository.GetComboStatus(),
                FlightNumber = flight.FlightNumber,
                From = flight.From.Name,
                To = flight.To.Name,
                Departure = flight.Departure,
                Arrival = flight.Arrival,
                Airplane = flight.AirPlane.AirplaneModel,
                StatusId = flight.Status.Id,
            };

            if (model.StatusId == 2 || model.StatusId == 3) // Voos terminados ou cancelados não podem ser editados
            {
                // Obter o state active
                var status = _context.Status.Where(x => x.Id == model.StatusId).FirstOrDefault();
                ViewBag.message = $"Flight is {status.StatusName}! Can't be edited";
                return View(model);
            }

            ViewBag.minDate = DateTime.Now;
            ViewBag.format = "dd/MM/yyyy HH:mm";
            return View(model);
        }

        // POST: Flight/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFlight(CreateFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Saber se o estado mudou (só se pode alterar de activo para canceled)
                var newStatus = _context.Status.Where(x => x.Id == model.StatusId).FirstOrDefault();
                if (newStatus == null)
                {
                    return NotFound();
                }

                //Saber se o avião mudou
                var airplane = await _airPlaneRepository.GetAirplaneByName(model.Airplane);

                if (airplane == null)
                {
                    return NotFound();
                }

                bool isAirplaneChange = airplane.AirplaneModel == model.Airplane ? false : true;

                bool isAvailable = true;

                if (isAirplaneChange) // Se o avião mudou tenho que verificar a disponibilidade do novo avião
                {
                    isAvailable = _flightRepository.AirplaneIsAvailable(airplane.Id, model.Departure, model.Arrival);
                }

                var to = await _flightRepository.GetAirportByName(model.To);
                var from = await _flightRepository.GetAirportByName(model.From);

                if (isAvailable)
                {
                    Flight flight = new Flight()
                    {
                        Id = model.Id,
                        FlightNumber = model.FlightNumber,
                        From = from,
                        To = to,
                        Departure = model.Departure,
                        Arrival = model.Arrival,
                        AirPlane = airplane,
                        Status = newStatus,
                        BusyEconomicSeats = airplane.EconomySeats,
                        //BusyExecutiveSeats = airplane.ExecutiveSeats,
                        BusyFirstClassSeats = airplane.FirstClassSeats,

                    };

                        await _flightRepository.UpdateAsync(flight);
                    //try
                    //{

                    //    List<Ticket> ticketList = _flightRepository.GetTickets(flight.Id);


                    //    //Depois de Fazer o update, enviar um email para todos os utilizadores com bilhetes, com os novos dados


                    //if (ticketList.Count != 0)
                    //    {

                    //        if (flight.Status.StatusName == "Active")
                    //        {
                    //            foreach (var item in ticketList)
                    //            {
                    //                _mailHelper.SendEmail(item.User.Email, "Flight changes", $"<h1>Flight changes</h1></br></br>" +
                    //                $"Please consider the new flight details:</br>" +
                    //                $"From: {item.Flight.From.FullName} </br>" +
                    //                $"To: {item.Flight.To.FullName} </br>" +
                    //                $"Departure: {item.Flight.Departure} </br>" +
                    //                $"Arrival: {item.Flight.Arrival} </br>" +
                    //                "Thank you for your attention");
                    //            }
                    //        }

                    //        else if (flight.Status.StatusName == "Canceled")
                    //        {

                    //            foreach (var item in ticketList)
                    //            {
                    //                _mailHelper.SendEmail(item.User.Email, "Flight canceled", $"<h1>Flight canceled</h1></br></br>" +
                    //                $"Your flight:</br>" +
                    //                $"From: {item.Flight.From.FullName} </br>" +
                    //                $"To: {item.Flight.To.FullName} </br>" +
                    //                $"Was canceled! Please, contact our customer service)");
                    //            }
                    //        }
                    //    }

                    return RedirectToAction(nameof(Index));
                    //}

                    //catch (Exception ex)
                    //{
                    //    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    //    GetCombos(model);
                    //    ViewBag.minDate = DateTime.Now;
                    //    ViewBag.format = "dd/MM/yyyy HH:mm";
                    //    return View(model);
                    //}
                }

                ViewBag.minDate = DateTime.Now;
                ViewBag.format = "dd/MM/yyyy HH:mm";
                GetCombos(model);
                ModelState.AddModelError(string.Empty, "Airplane isn't available. Choose another!");
                return View(model);
            }


            GetCombos(model);
            ViewBag.minDate = DateTime.Now;
            ViewBag.format = "dd/MM/yyyy HH:mm";
            return View(model);
        }

        // GET: Flight/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value); // .Value é obrigatório pois o id pode ser nulo

            if (flight == null)
            {
                return NotFound();
            }

            flight.To = await _flightRepository.GetAirportAsync(flight.To.Id);
            flight.From = await _flightRepository.GetAirportAsync(flight.From.Id);

            return View(flight);
        }

        // GET: Flight/Delete/5   
        public async Task<IActionResult> Delete(int? id)
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

            var to = await _flightRepository.GetAirportAsync(flight.To.Id);
            var from = await _flightRepository.GetAirportAsync(flight.From.Id);

            flight.From = from;
            flight.To = to;

            return View(flight);
        }

        // POST: Flight/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            try
            {
                await _flightRepository.DeleteAsync(flight); // Método já grava as alterações realizadas

                return RedirectToAction(nameof(Index));
            }

            catch (Exception) // Erro por algum motivo (bilhetes associados ao voo, por exemplo)
            {
                ViewBag.Message = "Flight with associated tickets. It can't be deleted!";

                return View();

            }
        }


        // Metodo para carregar todas as combobox
        private void GetCombos(CreateFlightViewModel model)
        {
            model.Airplanes = _flightRepository.GetComboAirplanes();
            model.Airports = _flightRepository.GetComboAirports();
            model.Status = _flightRepository.GetComboStatus();
        }

    }
}
