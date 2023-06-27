namespace GptEngineer.Infrastructure.Steps;

public interface IStep
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}