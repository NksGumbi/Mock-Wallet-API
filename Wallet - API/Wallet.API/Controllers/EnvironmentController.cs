using Microsoft.AspNetCore.Mvc;

namespace Wallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public EnvironmentController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("Environment")]
        public IActionResult GetEnvironment()
        {
            // Return the current environment name
            return Ok(new { Environment = _env.EnvironmentName });
        }
    }
}
