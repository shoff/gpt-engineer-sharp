namespace GptEngineer.Core.StepDefinitions;

public interface IGenerateEntrypoint
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}