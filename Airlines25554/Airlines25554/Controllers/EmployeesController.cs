using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Airlines25554.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Airlines25554.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailHelper _mailHelper;
        private readonly UserManager<User> _userManager;

        public EmployeesController(
            DataContext context,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IEmployeeRepository employeeRepository,
            IMailHelper mailHelper,
            UserManager<User> userManager )
        {
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _employeeRepository = employeeRepository;
            _mailHelper = mailHelper;
            _userManager = userManager;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
      

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if ((model.Username == null) || (model.Password == null) || (model.Email == null) || (model.Confirm == null))
            {
                // -> returnar mensagem de alerta para preencher todos os campos ---------------NÃO ESQUECER-----------------------------------------
                return View(model);
            }


            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(model.Username);
             
                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        UserName = model.Username
                    };

                    _context.Employees.Add(new Employee
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        DocumentId = model.DocumentId,
                        User = user
                    });

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The User couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Employee");
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _mailHelper.SendEmail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                   $"To allow the user, " +
                   $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");


                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow you user has been sent to email";
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "The User couldn't be logged.");

                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
                    }


                   var employee = _converterHelper.ToEmployee(model, imageId, true);


                    employee.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
                    await _employeeRepository.CreateAsync(employee);

                    return RedirectToAction(nameof(Index));

                }

            }
            return View(model);
        }

      
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var loggedUser = await _userManager.GetUserAsync(HttpContext.User); // -> Devolve o user que está logado

            var userId = loggedUser.Id; // id -> do user que está logado



            var employeeId = _context.Employees
                           .Include(u => u.User)
                           .Where(o => o.User == loggedUser)
                           .Select(i => i.Id).Single();

            id = Convert.ToInt32(employeeId);


            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEmployeeViewModel(employee);
   
            return View(model);
        }

        public async Task<IActionResult> EditEmployeeByAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEmployeeViewModel(employee);

            return View(model);
        }


        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
                    }

                    var employee = _converterHelper.ToEmployee(model, imageId, false);

                    employee.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
                    await _employeeRepository.UpdateAsync(employee);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _employeeRepository.ExistAsync(model.Id))
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

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
