namespace GptEngineer.Core;

public interface ISteps
{
    string SetupSysPrompt();
    Task<List<Dictionary<string, string>>> SimpleGen();
    Task<List<Dictionary<string, string>>> Clarify();
    Task<List<Dictionary<string, string>>> GenSpec();
    Task<List<Dictionary<string, string>>> Respec();
    Task<List<Dictionary<string, string>>> GenUnitTests();
    Task<List<Dictionary<string, string>>> GenClarifiedCode();
    Task<List<Dictionary<string, string>>> GenCode();
    Task<List<Dictionary<string, string>>> ExecuteUnitTests();
    Task<List<Dictionary<string, string>>> ExecuteEntrypoint();
    Task<List<Dictionary<string, string>>> GenEntrypoint();
    Task<List<Dictionary<string, string>>> UseFeedback();
    Task<List<Dictionary<string, string>>> FixCode();
    List<Tuple<string, string>> ParseChat(string chat);
}