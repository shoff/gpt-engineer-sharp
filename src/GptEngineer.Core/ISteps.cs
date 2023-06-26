namespace GptEngineer.Core;

public interface ISteps
{
    string SetupSysPrompt();
    Task<IEnumerable<Dictionary<string, string>>> SimpleGen();
    Task<IEnumerable<Dictionary<string, string>>> Clarify();
    Task<IEnumerable<Dictionary<string, string>>> GenSpec();
    Task<IEnumerable<Dictionary<string, string>>> Respec();
    Task<IEnumerable<Dictionary<string, string>>> GenUnitTests();
    Task<IEnumerable<Dictionary<string, string>>> GenClarifiedCode();
    Task<IEnumerable<Dictionary<string, string>>> GenCode();
    Task<IEnumerable<Dictionary<string, string>>> ExecuteUnitTests();
    Task<IEnumerable<Dictionary<string, string>>> ExecuteEntrypoint();
    Task<IEnumerable<Dictionary<string, string>>> GenEntrypoint();
    Task<IEnumerable<Dictionary<string, string>>> UseFeedback();
    Task<IEnumerable<Dictionary<string, string>>> FixCode();
    IEnumerable<Tuple<string, string>> ParseChat(string chat);
}