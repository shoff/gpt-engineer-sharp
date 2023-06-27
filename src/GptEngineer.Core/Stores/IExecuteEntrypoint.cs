namespace GptEngineer.Infrastructure.StepDefinitions;

public interface IExecuteEntrypoint
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}