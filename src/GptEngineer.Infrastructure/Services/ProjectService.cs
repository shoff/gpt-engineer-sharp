using GptEngineer.Core.Projects;
using Microsoft.Extensions.Logging;

namespace GptEngineer.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectFactory projectFactory;

    public ProjectService(
        ILogger<ProjectService> logger,
        IProjectFactory projectFactory)
    {
        this.projectFactory = projectFactory;
    }

    public async Task<IEnumerable<Project>> GetProjectsAsync(string? projectDirectoryPath = null)
    {
        // HACK
        var projects = new List<Project>();
        projectDirectoryPath ??= @"../../../../../projects";
        var enumerationOptions = new EnumerationOptions()
        {
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            RecurseSubdirectories = false
        };

        try
        {
            // should contain top-level project directories
            var projectDirectories = Directory.GetDirectories(projectDirectoryPath, "*", enumerationOptions);

            foreach (var projectDirectory in projectDirectories)
            {
                var project = await this.projectFactory.GetAsync(projectDirectory);
                projects.Add(project);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return projects;
    }
}