using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Airlines25554.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        private readonly IFlashMessage _flashMessage;

        public CountriesController(
            ICountryRepository countryRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IFlashMessage flashMessage)
        {
            _countryRepository = countryRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;
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
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            return View(country);
        }

        //_____________________________________________________________ CITY DETAILS _________________________________________________________________________________________//
        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var city = await _countryRepository.GetCityWithAirportsAsync(id.Value);
            if (city == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            return View(city);
        }

        //______________________________________________________________ CREATE Airport ________________________________________________________________________________________//
        public async Task<IActionResult> AddAirport(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var city = await _countryRepository.GetCityAsync(id.Value);


            if (city == null)
            {
                return new NotFoundViewResult("CityNotFound");
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
            var city = await _countryRepository.GetCityAsync(model.CityId);

            model.CityName = city.Name;

            if (this.ModelState.IsValid)
            {

                try
                {
                    await _countryRepository.AddAirportAsync(model);
                    return RedirectToAction("DetailsCity", new { id = model.CityId });
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This Airport already exist!");
                }
            }

        

            return this.View(model);
        }

        //______________________________________________________________ CREATE CITY ________________________________________________________________________________________//
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var model = new CityViewModel { CountryId = country.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {


            if (this.ModelState.IsValid)
            {

                try
                {
                    await _countryRepository.AddCityAsync(model);
                    return RedirectToAction("Details", new { id = model.CountryId });
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This city already exist!");
                }
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
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                }

                var country = _converterHelper.ToCountry(model, imageId, true);

                try
                {
                    await _countryRepository.CreateAsync(country);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This country already exist!");
                }

                //return View(country);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        //___________________________________________________________________ EDIT COUNTRY _______________________________________________________________________________//

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var model = _converterHelper.ToCountryViewModel(country);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                    }

                    var country = _converterHelper.ToCountry(model, imageId, false);
                    await _countryRepository.UpdateAsync(country);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _countryRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("CountryNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        //____________________________________________________________________ EDIT CITY _________________________________________________________________________________//

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return new NotFoundViewResult("CityNotFound");
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
                return new NotFoundViewResult("AirportNotFound");
            }

            var airport = await _countryRepository.GetAirportAsync(id.Value);
            if (airport == null)
            {
                return new NotFoundViewResult("AirportNotFound");
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
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            await _countryRepository.DeleteAsync(country);
            return RedirectToAction(nameof(Index));
        }

        //___________________________________________________________________ APAGAR CITY _______________________________________________________________________________//

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CityNotFound");
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return new NotFoundViewResult("CityNotFound");
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

        public IActionResult CountryNotFound()
        {
            return View();
        }

        public IActionResult CityNotFound()
        {
            return View();
        }

        public IActionResult AirportNotFound()
        {
            return View();
        }
    }
}

