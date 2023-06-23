namespace GptEngineer.Core.Projects;

public interface IProjectFactory
{
    Task<Project> CreateAsync(string path);
}