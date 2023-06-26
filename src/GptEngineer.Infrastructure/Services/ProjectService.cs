namespace GptEngineer.Infrastructure.Services;

using Extensions;
using GptEngineer.Core.Projects;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> logger;
    private readonly IProjectFactory projectFactory;
    private readonly IFileSystem fileSystem;
    private readonly IDistributedCache cache;

    public ProjectService(
        ILogger<ProjectService> logger,
        IProjectFactory projectFactory,
        IFileSystem fileSystem,
        IDistributedCache cache)
    {
        this.logger = logger;
        this.projectFactory = projectFactory;
        this.fileSystem = fileSystem;
        this.cache = cache;
    }

    public async Task<IEnumerable<Project>> GetProjectsAsync(string? projectDirectoryPath = null)
    {
        this.cache.TryGetFromCache<List<Project>>("projects", out var projects);

        if (projects is { Count: > 0 })
        {
            return projects;
        }
        else
        {
            // hate this, the cache extension should handle this
            projects = new List<Project>();
        }

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
                var project = await this.projectFactory.CreateProjectAsync(projectDirectory);
                projects!.Add(project);
            }
        }
        catch (Exception e)
        {
            this.logger.LogError("{Message}", e.Message);
        }

        if (projects is { Count: > 0 })
        {
            this.cache.TryAddToCache("projects", projects);
        }

        if (projects is null or { Count: > 0 })
        {
            this.logger.LogError("unable to find any projects, returning empty collection");
        }

        return projects ?? new List<Project>();
    }
}