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

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] object organizer)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            return Created("/api/v2/organizers/1", organizer);
        }
    }
}