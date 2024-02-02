using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelPlanController : Controller
    {
        protected readonly ITravelService _service;
        public TravelPlanController(ITravelService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TravelPlanRequest request)
        {
            try
            {
                return Ok(await _service.GetTravelPlan(request));
            }
            catch(Exception)
            {
                return BadRequest(new { message = "error" });
            }
            
        }
    }
}