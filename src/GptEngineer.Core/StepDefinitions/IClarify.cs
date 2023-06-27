namespace GptEngineer.Infrastructure.StepDefinitions;

public interface IClarify
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}