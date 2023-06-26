namespace GptEngineer.Client.Services;

using Core.Projects;
using Microsoft.Extensions.Logging;

public class ProjectService : IProjectService
{
    private readonly IProjectFactory projectFactory;
    private readonly IFileSystem fileSystem;

    public ProjectService(
        ILogger<ProjectService> logger,
        IProjectFactory projectFactory,
        IFileSystem fileSystem)
    {
        this.projectFactory = projectFactory;
        this.fileSystem = fileSystem;
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
            var projectDirectories = this.fileSystem.GetDirectories(projectDirectoryPath, "*", enumerationOptions);

            foreach (var projectDirectory in projectDirectories)
            {
                var project = await this.projectFactory.CreateAsync(projectDirectory);
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