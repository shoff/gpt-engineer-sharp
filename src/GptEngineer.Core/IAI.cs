namespace GptEngineer;

public interface IAI
{
    Task<List<Dictionary<string, string>>> Start(string system, string user);
    Dictionary<string, string> FSystem(string msg);
    Dictionary<string, string> FUser(string msg);
    Dictionary<string, string> FAssistant(string msg);
    Task<List<Dictionary<string, string>>> Next(List<Dictionary<string, string>> messages, string? prompt = null);
    Task<List<Dictionary<string, string>>> NextAsync(List<Dictionary<string, string>> messages, string? prompt = null);
}