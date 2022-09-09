using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airlines25554.Controllers
{
    public class AirPlanesController : Controller
    {
        private readonly IAirPlaneRepository _airPlaneRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AirPlanesController(
            IAirPlaneRepository airPlaneRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _airPlaneRepository = airPlaneRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
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
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UpLoadImageAsync(model.ImageFile, "airplanes");
                }


                var airPlane = _converterHelper.ToAirPlane(model, path, true);

                //TODO: Modificar para o user que tiver logado
                airPlane.User = await _userHelper.GetUserByEmailAsync("ricardo.simao.1357@gmail.com");
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
                    var path = model.ImageUrl;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UpLoadImageAsync(model.ImageFile,"airplanes");
                    }

         
                    var airPlane = _converterHelper.ToAirPlane(model,path,false);
                    
                    //TODO: Modificar para o user que tiver logado
                    airPlane.User = await _userHelper.GetUserByEmailAsync("ricardo.simao.1357@gmail.com");
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
