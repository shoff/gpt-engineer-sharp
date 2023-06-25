using GptEngineer.Core.Configuration;
using GptEngineer.Core.Projects;
using GptEngineer.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GptEngineer.Infrastructure.Projects;

public class ProjectFactory : IProjectFactory
{
    private readonly ILogger<ProjectFactory> logger;
    private readonly IOptions<AIOptions> options;

    public ProjectFactory(
        ILogger<ProjectFactory> logger,
        IOptions<AIOptions> options)
    {
        this.logger = logger;
        options.Validate();
        // not using these yet, but I think we should for paths.
        this.options = options;
    }

    public async Task<Project> GetAsync(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        if (!path.StartsWith(this.options.Value.ProjectPath))
        {
            path = $"{this.options.Value.ProjectPath}{path}";
        }

        var project = new Project(path)
        {
            Path = path,
            Name = Path.GetFileNameWithoutExtension(path),
            Description = "TODO",
        };

        this.logger.LogInformation("Loading project {Name} from {Path}", project.Name, project.Path);
        await project.LoadAsync();
        return project;
    }
}