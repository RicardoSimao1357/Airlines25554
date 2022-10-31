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
        public async Task <IActionResult> Index(SearchFlightViewModel model)
        {

                model.Airports = _countryRepository.GetFullNameAirports();

            if(model.From == model.To)
            {
                ModelState.AddModelError("", "The destinations must be diferent!");
                 return View(model);
            }

            if (ModelState.IsValid)
            {
                var from = model.From;
                var to = model.To;
                var departure = model.Departure;
                var classId = model.ClassId;

                if (classId == 0)
                {
                    model.Class = null;
                }

                if (classId == 1)
                {
                    model.Class = "Economic";
                }

                if (classId == 2)
                {
                    model.Class = "First Class";
                }


                



            }

            return View();
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
