using System;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airlines25554.Controllers
{
    
    public class AirPlanesController : Controller
    {
        private readonly IAirPlaneRepository _airPlaneRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public AirPlanesController(
            IAirPlaneRepository airPlaneRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _airPlaneRepository = airPlaneRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: AirPlanes
        public IActionResult Index()
        {
            return View(_airPlaneRepository.GetAll().OrderBy(p => p.AirplaneModel));
        }

        // GET: AirPlanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);

            if (airPlane == null)
            {
                return NotFound();
            }

            return View(airPlane);
        }

        [Authorize]
        // GET: AirPlanes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirPlanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirPlaneViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                }


                var airPlane = _converterHelper.ToAirPlane(model, imageId, true);

                //TODO: Modificar para o user que tiver logado
                airPlane.User = await _userHelper.GetUserByUserNameAsync("RicardoSimao");
                await _airPlaneRepository.CreateAsync(airPlane);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Authorize] 
        // GET: AirPlanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);
            if (airPlane == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToAirPlaneViewModel(airPlane);
            return View(model);
        }



        // POST: AirPlanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AirPlaneViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                    }

                    var airPlane = _converterHelper.ToAirPlane(model, imageId, false);

                    //TODO: Modificar para o user que tiver logado
                    airPlane.User = await _userHelper.GetUserByUserNameAsync("RicardoSimao");
                    await _airPlaneRepository.UpdateAsync(airPlane);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airPlaneRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
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

        // GET: AirPlanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);

            if (airPlane == null)
            {
                return NotFound();
            }

            return View(airPlane);
        }

        // POST: AirPlanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPlane = await _airPlaneRepository.GetByIdAsync(id);
            await _airPlaneRepository.DeleteAsync(airPlane);
            return RedirectToAction(nameof(Index));
        }
    }
}
