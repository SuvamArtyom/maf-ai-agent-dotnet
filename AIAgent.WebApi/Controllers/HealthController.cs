using Microsoft.AspNetCore.Mvc;

namespace AIAgent.WebApi.Controllers
{
    /// <summary>
    /// Health check controller to verify API status.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> logger;

        /// <summary>
        /// Initializes a new instance of <see cref="HealthController"/>.
        /// </summary>
        public HealthController(ILogger<HealthController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Returns service health status and current UTC server time.
        /// </summary>
        /// <response code="200">API is healthy and running.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealth()
        {
            logger.LogInformation("Health probe verified.");
            return Ok(new
            {
                Status = "Healthy",
                Service = "KnowledgeAgent Web API",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                TimestampUtc = DateTime.UtcNow
            });
        }
    }
}

