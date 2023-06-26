using Microsoft.AspNetCore.Mvc;

namespace GptEngineer.API.Controllers;

[Route("api/v1/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogger<LogController> logger;

    public LogController(ILogger<LogController> logger)
    {
        this.logger = logger;
    }

    [HttpPost("error")]
    public IActionResult LogError([FromBody]string message)
    {
        this.logger.LogError("{Message}", message);
        return Ok();
    }
}