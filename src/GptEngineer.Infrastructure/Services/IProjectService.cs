namespace GptEngineer.Client.Services;

using Core;
using GptEngineer.Core.Projects;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsAsync(string? projectDirectory);
}