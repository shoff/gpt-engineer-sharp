namespace GptEngineer.Infrastructure.Services;

using GptEngineer.Core.Projects;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsAsync(string? projectDirectory);
}