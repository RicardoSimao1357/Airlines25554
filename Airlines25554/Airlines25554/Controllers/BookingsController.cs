using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
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
           



            //ViewBag.passengerViewModel = model;

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


            ViewBag.Email = model.Email;
            
            model.FlightId = flight.Id;
            model.Classes = list;
            //model.Class = list.va
            model.TotalSeatsList = ticketsList.ToList();


            return View(model);

        }


        public async Task<IActionResult> ShowTicket(PassengerViewModel model)
        {

          
           var ticket = await _ticketRepository.GetByIdAsync(model.Id);

            model.ClassName = ticket.Class;
            model.Seat = ticket.Seat;

     
            if (ModelState.IsValid)
            {

                var flight = await _flightRepository.GetFlightWithObjectsAsync(model.FlightId);




                var destinationFrom = flight.From;
                var destinationTo =  flight.To; 

                var from =  await  _countryRepository.GetCityWithAirportAsync(destinationFrom);
                var to = await _countryRepository.GetCityWithAirportAsync(destinationTo);

                model.From = from.Name;
                model.To = to.Name;
                model.Date = flight.Departure.ToShortDateString();
                model.Time = flight.Departure.ToShortTimeString();
                model.FirstName = model.FirstName;
                model.LastName = model.LastName;
                model.Email = model.Email;
                model.PassportId = model.PassportId;
                return View(model);

            }

            return this.RedirectToAction("Index", "Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTicket(PassengerViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    Ticket ticket = new Ticket();

            //    User user = await _userHelper.GetUserByEmailAsync(model.UserEmail);

            //    Flight flight = _flightRepository.GetFlight(model.FlightId);

            //    ticket.Seat = model.Seat;
            //    ticket.User = user;
            //    ticket.Flight = flight;

            //    if (model.Class == 1)
            //    {
            //        ticket.Class = "Economic";
            //    }

            //    if (model.Class == 2)
            //    {
            //        ticket.Class = "Business";
            //    }

            //    try
            //    {
            //        await _ticketRepository.CreateAsync(ticket);// Ao usar o create grava logo

            //        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

            //        // Criar um link que vai levar lá dentro uma acção. Quando o utilizador carregar neste link, 
            //        // vai no controlador Account executar a action "ConfirmEmail"(Ainda será feita)
            //        // Este ConfirmEmail vai receber um objecto novo que terá um userid e um token.
            //        var tokenLink = this.Url.Action("Index", "Home", new
            //        {
            //            userid = user.Id,
            //            token = myToken,

            //        }, protocol: HttpContext.Request.Scheme);

            //        _mailHelper.SendMail(model.UserEmail, "Ticket", $"<h1>Ticket Confirmation</h1>" +
            //           $"Your ticket information, " +
            //           $"Flight: {model.FlightId}, " +
            //           $"Class: {ticket.Class}, " +
            //           $"Date: {ticket.Seat}, " +
            //           $"Click in this link to home page :</br></br><a href = \"{tokenLink}\">Airline</a>");


            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
            //        return View(model);
            //    }

            //}

            return this.RedirectToAction("Index", "Flights");
        }
    }
}
