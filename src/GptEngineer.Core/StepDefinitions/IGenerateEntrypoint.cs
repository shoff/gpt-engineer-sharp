namespace GptEngineer.Infrastructure.StepDefinitions;

public interface IGenerateEntrypoint
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}