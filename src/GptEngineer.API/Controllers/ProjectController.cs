using Microsoft.AspNetCore.Authorization;

namespace GptEngineer.API.Controllers;

using Client.Services;
using Core.Configuration;
using Core.Projects;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[Route("api/v1/project")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> logger;
    private readonly IOptions<AIOptions> options;
    private readonly IProjectService projectService;

    public ProjectController(
        ILogger<ProjectController> logger,
        IOptions<AIOptions> options,
        IProjectService projectService)
    {
        this.logger = logger;
        this.options = options;
        this.projectService = projectService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async IAsyncEnumerable<Project> GetAsync()
    {
        var directory = this.options.Value.ProjectPath;

        if (directory is null || !Directory.Exists(directory))
        {
            this.logger.LogError("ProjectPath is null or does not exist");
            yield break;
        }
        var projects = await this.projectService.GetProjectsAsync(directory);
        foreach (var project in projects)
        {
            await Task.Yield(); // This line is important to ensure the method yields control back to the caller before continuing the loop
            yield return project;
        }
    }
}