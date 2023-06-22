namespace GptEngineer.API.Controllers;

using Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[Route("api/v1/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> logger;
    private readonly IOptions<AIOptions> options;

    public ProjectController(
        ILogger<ProjectController> logger,
        IOptions<AIOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var directory = this.options.Value.ProjectPath;
        if (directory == null)
        {
            this.logger.LogError("ProjectPath is null");
            return new StatusCodeResult(500);
        }

        if (!Directory.Exists(directory))
        {
            // create directory
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to create directory");
                return new StatusCodeResult(500);
            }

            return this.Ok(new List<string>());
        }

        // read all of the projects available
        try
        {
            var files = Directory.GetFiles(directory);
            return this.Ok(files.Select(Path.GetFileName));
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to read directory");
            return new StatusCodeResult(500);
        }
    }
}