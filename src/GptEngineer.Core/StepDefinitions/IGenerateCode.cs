namespace GptEngineer.Core.StepDefinitions;

public interface IGenerateCode
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}