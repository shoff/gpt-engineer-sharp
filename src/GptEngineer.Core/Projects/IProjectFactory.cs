namespace GptEngineer.Core.Projects;

public interface IProjectFactory
{
    Task<Project> GetAsync(string path);
}