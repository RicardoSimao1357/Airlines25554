using Airlines25554.Data;
using Airlines25554.Data.Entities;
using Airlines25554.Helpers;
using Airlines25554.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Airlines25554.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            DataContext context,
            IUserHelper userHelper,
            IConfiguration configuration,
            ICustomerRepository customerRepository,
            IMailHelper  mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _configuration = configuration;
            _customerRepository = customerRepository;
            _mailHelper = mailHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
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

                    var result = await _userHelper.AddUserAsync(user, model.Password);


                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The User couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Customer");

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);


                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _mailHelper.SendEmail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                   $"To allow the user, " +
                   $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");


                    var customer = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,    
                        PassportId = model.PassportId,  
                        User = user,
                    };

                    if (response.IsSuccess)
                    {
                        await _customerRepository.CreateAsync(customer);
                        ViewBag.Message = "The instructions to allow you user has been sent to email";
                        return RedirectToAction("Index","Home");
                    }

                    ModelState.AddModelError(string.Empty, "The User couldn't be created");
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Customer");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Customer");
                }

                await _context.SaveChangesAsync();

            }
            return View(model);
        }

  

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
             //   model.FirstName = user.FirstName;
             //   model.LastName = user.LastName;
            }

            return View(model);
        }


        [HttpPost]

        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);

                if (user != null)
                {
                  //  user.FirstName = model.FirstName;
                  //  user.LastName = model.LastName;
                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }


                }

            }

            return View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);

                if (user != null)
                {

                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found");
                }
            }


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(model.Username); // -> Verificar se o username existe
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(     //
                        user,                                                 // -> Validar a password
                        model.Password);                                      //

                    if (result.Succeeded)
                    {
                        var claims = new[]                                                     // -> Contrução do Token
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),                // -> Regista o email do utilizador
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  // -> Gera um guid que fica associado ao email
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // -> SymmetricSecurityKey = Tipo de encriptação
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // -> Gera o token com a key gerada em cima
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),      // -> Validade do token 
                            signingCredentials: credentials);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return RedirectToAction("Login","Account");
        }
    }
}
