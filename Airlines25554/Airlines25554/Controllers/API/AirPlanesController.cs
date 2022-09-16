using Airlines25554.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Airlines25554.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class AirPlanesController : Controller
    {
        private readonly IAirPlaneRepository _airPlaneRepository;

        public AirPlanesController(IAirPlaneRepository airPlaneRepository )
        {
            _airPlaneRepository = airPlaneRepository;
        }

        [HttpGet]   
        public IActionResult GetAirPlanes()
        {
            return Ok( _airPlaneRepository.GetAllWithUsers());   
        }
    }
}
