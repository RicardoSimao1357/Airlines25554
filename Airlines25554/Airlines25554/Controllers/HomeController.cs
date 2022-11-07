using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IFlightRepository flightRepository,
            ICountryRepository countryRepository

            )
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
        }

        public IActionResult Index()
        {
            var model = new SearchFlightViewModel()
            {
                Classes = _flightRepository.GetComboClasses(),
                Airports = _countryRepository.GetFullNameAirports()
            };


            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SearchFlightViewModel model)
        {

            var departure = model.Departure;

            var date  = model.Departure.ToShortDateString();    

            //var fromAirport = _flightRepository.GetAirportAsync(from.va);

            model.Airports = _countryRepository.GetFullNameAirports();

            if (departure <= @DateTime.Now)
            {
                ModelState.AddModelError("", "Insert a valid date!");
                model.Classes = _flightRepository.GetComboClasses();
                model.Airports = _countryRepository.GetFullNameAirports();
                return View(model);
            }

            if ((model.FromId != null) && (model.ToId != null))
            {

                if (model.FromId == model.ToId)
                {
                    ModelState.AddModelError("", "The destinations must be diferent!");
                    model.Classes = _flightRepository.GetComboClasses();
                    model.Airports = _countryRepository.GetFullNameAirports();
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {


                if ((model.FromId == null) && (model.ToId != null))
                {
                    model.Flights = _flightRepository.GetSearchedFlightByToDestinationAndDate(model.ToId, departure);
                    
                    
                }
                else if ((model.ToId == null) && (model.FromId != null))
                {
                    model.Flights = _flightRepository.GetSearchedFlightByFromDestinationAndDate(model.FromId, departure);

                }
                else if (model.FromId == null && model.ToId == null)
                {
                    model.Flights = _flightRepository.GetSearchedFlightByDate(departure);

                }
                else
                {
                    model.Flights = _flightRepository.GetSearchedFlightAsync(model.FromId, model.ToId, departure);
                }

                if(model.Flights.Count == 0)
                {
                    this.ModelState.AddModelError(string.Empty, "Sorry, there are no flights matching your search.");
                    model.Classes = _flightRepository.GetComboClasses();
                    model.Airports = _countryRepository.GetFullNameAirports();
                    return View(model);
                }

                TempData.Put("FlightsList", model);
                return RedirectToAction("SearchedFlight", "Home");


             
            }

            return View(model);
        }

        public IActionResult SearchedFlight()
        {
            // Agarrar o modelo

            var data = TempData.Get<SearchFlightViewModel>("FlightsList");

            if (data == null)
            {
                return NotFound();
            }

            SearchFlightViewModel model = new SearchFlightViewModel();

            model.flightId = data.flightId;
            model.Flights = data.Flights;
       
            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
