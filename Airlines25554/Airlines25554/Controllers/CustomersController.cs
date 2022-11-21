using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Airlines25554.Models;

namespace Airlines25554.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ITicketPurchasedRepository _ticketPurchasedRepository;
   

        public CustomersController(
            DataContext context,
            IBlobHelper blobHelper,
            ICustomerRepository customerRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            ITicketPurchasedRepository ticketPurchasedRepository )
        {
            _context = context;
            _blobHelper = blobHelper;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _ticketPurchasedRepository = ticketPurchasedRepository;

        }

        // GET: Customers
        public IActionResult Index()
        {
            return View(_customerRepository.GetAll());

        }

      
        public  async Task<IActionResult> Historic()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); // -> Devolve o user que está logado

            return View(_ticketPurchasedRepository.BoughtTicketsByUser(user));

        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

      



       // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string username)
        {
            var user = await _customerRepository.GetUserByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.GetCustomerByUserAsync(user);

            if(customer == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToCustomerViewModel(customer);

            return View(model);
        }

      
        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    var customer = _converterHelper.ToCustomer(model, imageId, false);



                    customer.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
                    await _customerRepository.UpdateAsync(customer);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _customerRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
