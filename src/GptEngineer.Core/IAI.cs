using GptEngineer.Core.Events;

namespace GptEngineer.Core;

public interface IAI : IDisposable
{
    Task<List<Dictionary<string, string>>> Start(string system, string user);
    Dictionary<string, string> AsSystemRole(string msg);
    Dictionary<string, string> AsUserRole(string msg);
    Dictionary<string, string> AsAssistantRole(string msg);
    Task<List<Dictionary<string, string>>> Next(List<Dictionary<string, string>> messages, string? prompt = null);
    Task<List<Dictionary<string, string>>> NextAsync(List<Dictionary<string, string>> messages, string? prompt = null);

    // events
    event EventHandler<ChatCompletionEventArgs>? CompletionReceived;
}