namespace GptEngineer.Core.StepDefinitions;

public interface IStep
{
    public Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}