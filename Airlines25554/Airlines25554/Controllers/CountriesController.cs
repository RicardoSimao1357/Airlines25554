using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;
    //    private readonly IFlashMessage _flashMessage;

        public CountriesController(
            ICountryRepository countryRepository)
        //    IFlashMessage flashMessage)
        {
            _countryRepository = countryRepository;
        //    _flashMessage = flashMessage;
        }



        public IActionResult Index()
        {
            return View(_countryRepository.GetCountriesWithCities());
        }

        //______________________________________________________________ COUNTRY DETAILS _____________________________________________________________________________________//
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        //_____________________________________________________________ CITY DETAILS _________________________________________________________________________________________//
        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _countryRepository.GetCityWithAirportsAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        //______________________________________________________________ CREATE Airport ________________________________________________________________________________________//
        public async Task<IActionResult> AddAirport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _countryRepository.GetCityAsync(id.Value );

        //    var city = await 
            if (city == null)
            {
                return NotFound();
            }

            var model = new AirportViewModel
            { 
                CityId = city.Id, 
                
                };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAirport(AirportViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _countryRepository.AddAirportAsync(model);
                return RedirectToAction("DetailsCity", new { id = model.CityId });
            }

            return this.View(model);
        }

        //______________________________________________________________ CREATE CITY ________________________________________________________________________________________//
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            var model = new CityViewModel { CountryId = country.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _countryRepository.AddCityAsync(model);
                return RedirectToAction("Details", new { id = model.CountryId });
            }

            return this.View(model);
        }

        //__________________________________________________________________ CREATE COUNTRY ____________________________________________________________________________//
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _countryRepository.CreateAsync(country);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //_flashMessage.Danger("This country already exist!");
                }

                return View(country);
            }

            return View(country);
        }
          
        //___________________________________________________________________ EDIT COUNTRY _______________________________________________________________________________//

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryRepository.UpdateAsync(country);
                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }


        //____________________________________________________________________ EDIT CITY _________________________________________________________________________________//

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }


        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {
            if (this.ModelState.IsValid)
            {
                var countryId = await _countryRepository.UpdateCityAsync(city);
                if (countryId != 0)
                {
                    return this.RedirectToAction($"Details", new { id = countryId });
                }
            }

            return this.View(city);
        }

        //____________________________________________________________________ EDIT Airport _________________________________________________________________________________//

        public async Task<IActionResult> EditAirport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _countryRepository.GetAirportAsync(id.Value);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }


        [HttpPost]
        public async Task<IActionResult> EditAirport(Airport airport)
        {
            if (this.ModelState.IsValid)
            {
                var cityId = await _countryRepository.UpdateAirportAsync(airport);
                if (cityId != 0)
                {
                    return this.RedirectToAction($"DetailsCity", new { id = cityId });
                }
            }

            return this.View(airport);
        }

        //___________________________________________________________________ APAGAR COUNTRY ____________________________________________________________________________//

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            await _countryRepository.DeleteAsync(country);
            return RedirectToAction(nameof(Index));
        }

        //___________________________________________________________________ APAGAR CITY _______________________________________________________________________________//

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var countryId = await _countryRepository.DeleteCityAsync(city);
            return this.RedirectToAction($"Details", new { id = countryId });
        }

        //___________________________________________________________________ APAGAR AIRPLANE _______________________________________________________________________________//

        public async Task<IActionResult> DeleteAirport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _countryRepository.GetAirportAsync(id.Value);
            if (airport == null)
            {
                return NotFound();
            }

            var cityId = await _countryRepository.DeleteAirportAsync(airport);
            return this.RedirectToAction($"DetailsCity", new { id = cityId });
        }
    }
}

