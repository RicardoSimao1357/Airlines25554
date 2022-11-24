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
    [Authorize(Roles = "Admin")] // -> Apenas o Admin  faz CRUD dos Aviões

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
                return new NotFoundViewResult("AirplaneNotFound");
 
            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);

            if (airPlane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
 
            }

            return View(airPlane);
        }


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


                airPlane.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
                await _airPlaneRepository.CreateAsync(airPlane);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: AirPlanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");

            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);
            if (airPlane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
 
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

                    airPlane.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
                    await _airPlaneRepository.UpdateAsync(airPlane);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airPlaneRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("AirplaneNotFound");
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            var airPlane = await _airPlaneRepository.GetByIdAsync(id.Value);

            if (airPlane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            return View(airPlane);
        }

        // POST: AirPlanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPlane = await _airPlaneRepository.GetByIdAsync(id);

            try
            {
                //throw new Exception("Excepção de Teste");
                await _airPlaneRepository.DeleteAsync(airPlane);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{airPlane.AirplaneModel} it's probably being used!!";
                    ViewBag.ErrorMessage = $"{airPlane.AirplaneModel} cannot be deleted as there are flights scheduled for this plane.</br></br>" +
                        $"try to delete the flight first!";
       
                }

                return View("Error");
            }
        }

        public IActionResult AirplaneNotFound()
        {
            return View();
        }
    }
}
