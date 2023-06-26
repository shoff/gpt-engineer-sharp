using GptEngineer.Core.Projects;

namespace GptEngineer.Infrastructure.Services;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsAsync(string? projectDirectory);
}