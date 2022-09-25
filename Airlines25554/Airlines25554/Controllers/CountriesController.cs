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
      //  private readonly IFlashMessage _flashMessage;

        public CountriesController(
            ICountryRepository countryRepository)
       //     IFlashMessage flashMessage
        {
            _countryRepository = countryRepository;
       //     _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            return View(_countryRepository.GetCountriesWithAirports());
        }


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

            var countryId = await _countryRepository.DeleteAirportAsync(airport);
            return this.RedirectToAction($"Details", new { id = countryId });
        }

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
                var countryId = await _countryRepository.UpdateAirportAsync(airport);
                if (countryId != 0)
                {
                    return this.RedirectToAction($"Details", new { id = countryId });
                }
            }

            return this.View(airport);
        }

        public async Task<IActionResult> AddAirport(int? id)
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

            var model = new AirportViewModel { CountryId = country.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAirport(AirportViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _countryRepository.AddAirportAsync(model);
                return RedirectToAction("Details", new { id = model.CountryId });
            }

            return this.View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetCountryWithAirportsAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

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
                 //   _flashMessage.Danger("This country already exist!");
                }

                return View(country);
            }

            return View(country);
        }

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
    }
}

