namespace GptEngineer.Core.StepDefinitions;

public interface IClarify
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}