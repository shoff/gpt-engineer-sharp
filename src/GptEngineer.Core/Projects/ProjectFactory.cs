namespace GptEngineer.Core.Projects;

using Microsoft.Extensions.Logging;

public class ProjectFactory : IProjectFactory
{
    private readonly ILogger<ProjectFactory> logger;

    public ProjectFactory(ILogger<ProjectFactory> logger)
    {
        this.logger = logger;
    }

    public async Task<Project> CreateAsync(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        var project = new Project(path)
        {
            Path = path,
            Name = Path.GetFileNameWithoutExtension(path),
            Description = "TODO",
        };

        this.logger.LogInformation($"Loading project {project.Name} from {project.Path}");
        await project.LoadAsync();
        return project;
    }
}