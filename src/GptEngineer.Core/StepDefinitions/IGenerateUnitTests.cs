namespace GptEngineer.Infrastructure.StepDefinitions;

public interface IGenerateUnitTests
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}