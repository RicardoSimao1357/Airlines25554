using Airlines25554.Data;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public HomeController(
            ILogger<HomeController> logger,
            IFlightRepository flightRepository,
            ICountryRepository countryRepository,
            ITicketRepository ticketRepository,
            ICustomerRepository customerRepository,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
            _ticketRepository = ticketRepository;
            _customerRepository = customerRepository;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            var model = new SearchFlightViewModel()
            {
                Classes = _flightRepository.GetComboClasses(),
                Airports = _countryRepository.GetFullNameAirports(),

            };


            return View(model);
        }

        public IActionResult ShowFlights()
        {

            var model = new FlightViewModel()
            {
                States = _flightRepository.GetComboStatus(), // Apresentar uma combobox com os estado dos voos para posteriormente apresentar os voos na tabela
                Flights = _flightRepository.GetAllWithObjects(),
                Classes = _flightRepository.GetComboClasses(),
            };

            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SearchFlightViewModel model)
        {

            var departure = model.Departure;

            var date = model.Departure.ToShortDateString();

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

                if (model.Flights.Count == 0)
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
                return new NotFoundViewResult("FlightNotFound");
            }

            SearchFlightViewModel model = new SearchFlightViewModel();

            model.flightId = data.flightId;
            model.Flights = data.Flights;
            model.Classes = _flightRepository.GetComboClasses();


            return View(model);
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult FlightNotFound()
        {
            return View();
        }


    }
}
