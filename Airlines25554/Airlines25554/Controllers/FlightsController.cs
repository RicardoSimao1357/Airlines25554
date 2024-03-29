﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

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
        private readonly ITicketPurchasedRepository _ticketPurchasedRepository;
        private readonly IFlashMessage _flashMessage;
        private Random _random; 

        public FlightsController(
            DataContext context,
            ICountryRepository countryRepository,
            IFlightRepository flightRepository,
            IAirPlaneRepository airPlaneRepository,
            IMailHelper mailHelper,
            ITicketRepository ticketRepository,
            ITicketPurchasedRepository ticketPurchasedRepository,
            IFlashMessage flashMessage
            )
        {
            _context = context;
            _countryRepository = countryRepository;
            _flightRepository = flightRepository;
            _airPlaneRepository = airPlaneRepository;
            _mailHelper = mailHelper;
            _ticketRepository = ticketRepository;
            _ticketPurchasedRepository = ticketPurchasedRepository;
            _flashMessage = flashMessage;
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
                    return new NotFoundViewResult("AirplaneNotFound");
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
                        BusyExecutiveSeats = airplane.ExecutiveSeats,
                        BusyFirstClassSeats = airplane.FirstClassSeats,
                        EconomicTicketPrice = model.EconomicTicketPrice,
                        ExecutiveTicketPrice = model.ExecutiveTicketPrice,
                        FirstClassTicketPrice = model.FirstClassTicketPrice,    

                    };

                    var economicSeats = flight.BusyEconomicSeats;
                    var executiveSeats = flight.BusyExecutiveSeats;
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
                                Price = flight.EconomicTicketPrice,
                                IsAvailable = true,
                            };


                            await _ticketRepository.CreateAsync(economicTickets);
                        }

                        for (int i = 0; i < executiveSeats; i++)
                        {

                            Ticket executiveTickets = new Ticket()
                            {
                                Seat = (i + 1) + "EXE",
                                Class = "Executive",
                                Flight = flight,
                                Price = flight.ExecutiveTicketPrice,
                                IsAvailable = true,
                            };


                            await _ticketRepository.CreateAsync(executiveTickets);
                        }

                        for (int i = 0; i < firstClassSeats; i++)
                        {

                            Ticket firstClassTickets = new Ticket()
                            {
                                Seat = (i + 1) + "FIRST",
                                Class = "First Class",
                                Flight = flight,
                                Price = flight.FirstClassTicketPrice,   
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
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value);
          

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var ticketList =  _ticketPurchasedRepository.TicketListByFlight(flight);

            if (ticketList != null)
            {
                ViewBag.Message = "This Flight has already tickets bought, Impossible editing!";
                return View();
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
                    return new NotFoundViewResult("FlightNotFound");
                }

                //Saber se o avião mudou
                var airplane = await _airPlaneRepository.GetAirplaneByName(model.Airplane);

                if (airplane == null)
                {
                    return new NotFoundViewResult("FlightNotFound");
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
                        BusyExecutiveSeats = airplane.ExecutiveSeats,
                        BusyFirstClassSeats = airplane.FirstClassSeats,

                    };

                        await _flightRepository.UpdateAsync(flight);
                  

                    return RedirectToAction(nameof(Index));
                
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
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value); // .Value é obrigatório pois o id pode ser nulo

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
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
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetFlightWithObjectsAsync(id.Value);

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
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
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetByIdAsync(id);

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
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

        public IActionResult FlightNotFound()
        {
            return View();
        }

        public IActionResult AirplaneNotFound()
        {
            return View();
        }

    }
}
