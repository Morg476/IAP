using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class OrganizersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var organizers = new[]
            {
                new { Id = 1, Name = "Org A" },
                new { Id = 2, Name = "Org B" }
            };

            return Ok(organizers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] object organizer)
        {
            return Created("/api/v2/organizers/1", organizer);
        }
    }
}