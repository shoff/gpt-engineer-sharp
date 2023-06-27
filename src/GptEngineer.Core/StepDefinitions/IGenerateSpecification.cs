namespace GptEngineer.Core.StepDefinitions;

public interface IGenerateSpecification
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}