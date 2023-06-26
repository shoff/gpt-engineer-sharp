namespace GptEngineer.Core.Projects;

public interface IProjectFactory
{
    Task<Project> CreateProjectAsync(string path);
}