namespace GptEngineer.Core.Stores;

public interface IExecuteEntrypoint
{
    Task<IEnumerable<Dictionary<string, string>>> RunAsync();
}