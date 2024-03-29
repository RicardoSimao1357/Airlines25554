﻿using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

        public IActionResult Index()
        {

            var model = new FlightViewModel()
            {
                States = _flightRepository.GetComboStatus(), // Apresentar uma combobox com os estado dos voos para posteriormente apresentar os voos na tabela
                Flights = _flightRepository.GetAllWithObjects(),
                Classes = _flightRepository.GetComboClasses(),  
            };

            return View(model);
        }


        public IActionResult PassengerData(int? id, string ticketClass)
        {

            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            //var flight = _flightRepository.GetByIdAsync(id.Value);

            var ticketList = _ticketRepository.AvailableFlightTickets(id.Value);

            if(ticketList.Count == 0)
            {
                RedirectToAction("Index");
            }

            var model = new PassengerViewModel()
            {
                FlightId = id.Value,
                Class = ticketClass
        
                
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Booking(PassengerViewModel model)
        {

            var flight = await _flightRepository.GetFlightWithObjectsAsync(model.FlightId);

                                                

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }


            if (flight.Status.StatusName == "Canceled" || flight.Status.StatusName == "Concluded")
            {
                ViewBag.Message = "The flight isn't active! It´s impossible to create tickets!";
                return View();
            }


            var ticketsList = _ticketRepository.FlightTickets(model.FlightId, model.Class);


            model.FlightId = flight.Id;
   
            model.TotalSeatsList = ticketsList.ToList();
            model.From = flight.From.CityName;
            model.To = flight.To.CityName;
            model.IATAFrom = flight.From.IATA;
            model.IATATo = flight.To.IATA;
            model.Date = flight.Departure.ToShortDateString();
            model.Time = flight.Departure.ToShortTimeString();



            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ShowTicket(PassengerViewModel model)
        {
            var ticket = await _ticketRepository.GetByIdAsync(model.Id);

            model.Seat = ticket.Seat;
            model.Class = ticket.Class;



            //if (ModelState.IsValid)
            //{

            //    var flight = await _flightRepository.GetFlightWithObjectsAsync(model.FlightId);

            //    var destinationFrom = flight.From;
            //    var destinationTo = flight.To;

            //    var from = await _countryRepository.GetCityWithAirportAsync(destinationFrom);
            //    var to = await _countryRepository.GetCityWithAirportAsync(destinationTo);

            //    model.From = from.Name;
            //    model.To = to.Name;

            //    model.Class = ticket.Class;
            //    model.Seat = ticket.Seat;

            //    return View(model);

            //}

            return View(model);

            //return this.RedirectToAction("Index", "Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTicket(PassengerViewModel model)
        {
            var flight = _flightRepository.GetFlight(model.FlightId);

            var ticket = _ticketRepository.GetTicketById(model.Id);
            model.Class = ticket.Class;


            if (ModelState.IsValid)
            {
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User); // -> Devolve o user que está logado

                //var userId = loggedUser.Id; // id -> do user que está logado

                //var flight = _flightRepository.GetFlight(model.FlightId);

                Passenger passenger = new Passenger()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PassportId = model.PassportId,
                    User = loggedUser,

                };
                    await _passengerRepository.CreateAsync(passenger);

                TicketPurchased ticketPurchased = new TicketPurchased()
                {
                    User = loggedUser,
                    Flight = flight,
                    Seat = model.Seat,
                    Class = model.Class,

                };


                try
                {
                    await _ticketPurchasedRepository.CreateAsync(ticketPurchased);
                   _ticketRepository.UpdateTicketIsAvailable(ticket);


                    _mailHelper.SendEmail(passenger.Email, "Ticket", $"<h1>Ticket Confirmation</h1>" +
                       $"Your ticket information, " +
                       $"Flight: {model.FlightId}, " +
                       $"Class: {ticket.Class}, " +
                       $"Date: {model.Date}, ");


                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    return View(model);
                }

            }

            return this.RedirectToAction("Index", "Booking");
        }

        public IActionResult FlightNotFound()
        {
            return View();
        }
    }
}

