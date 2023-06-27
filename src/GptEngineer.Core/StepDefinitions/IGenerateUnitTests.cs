namespace GptEngineer.Core.StepDefinitions;

public interface IGenerateUnitTests
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}