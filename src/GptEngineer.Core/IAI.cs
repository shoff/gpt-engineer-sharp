using GptEngineer.Core.Events;

namespace GptEngineer.Core;

public interface IAI
{
    Task<IEnumerable<Dictionary<string, string>>> Start(string system, string user);
    Dictionary<string, string> AsSystemRole(string msg);
    Dictionary<string, string> AsUserRole(string msg);
    Dictionary<string, string> AsAssistantRole(string message);
    Task<IEnumerable<Dictionary<string, string>>> NextAsync(IEnumerable<Dictionary<string, string>> messages, string? prompt = null);

    // events
    event EventHandler<ChatCompletionEventArgs>? CompletionReceived;
}