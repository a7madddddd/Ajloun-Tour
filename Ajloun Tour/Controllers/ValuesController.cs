using Ajloun_Tour.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ValuesController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() {

           var tours =  _context.TourOffers.ToList();
            return Ok(tours);
        }
    }
}
