using System.Linq;
using System.Threading.Tasks;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airlines25554.Controllers
{
    public class AirPlanesController : Controller
    {
        private readonly DataContext _context;

        public AirPlanesController(DataContext context)
        {
            _context = context;
        }

        // GET: AirPlanes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AirPlanes.ToListAsync());
        }

        // GET: AirPlanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _context.AirPlanes
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create(AirPlane airPlane)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airPlane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airPlane);
        }

        // GET: AirPlanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _context.AirPlanes.FindAsync(id);
            if (airPlane == null)
            {
                return NotFound();
            }
            return View(airPlane);
        }

        // POST: AirPlanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AirPlane airPlane)
        {
            if (id != airPlane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airPlane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPlaneExists(airPlane.Id))
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
            return View(airPlane);
        }

        // GET: AirPlanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPlane = await _context.AirPlanes
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var airPlane = await _context.AirPlanes.FindAsync(id);
            _context.AirPlanes.Remove(airPlane);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirPlaneExists(int id)
        {
            return _context.AirPlanes.Any(e => e.Id == id);
        }
    }
}
