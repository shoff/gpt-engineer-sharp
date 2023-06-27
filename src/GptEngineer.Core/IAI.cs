using GptEngineer.Core.Events;

namespace GptEngineer.Core;

public interface IAI
{
    event EventHandler<ChatCompletionEventArgs>? CompletionReceived;
    Task<IEnumerable<Dictionary<string, string>>> Start(string system, string user);
    Dictionary<string, string> AsRoleMessage(Role role, string? message);
    Task<IEnumerable<Dictionary<string, string>>> NextAsync(IEnumerable<Dictionary<string, string>> messages, string? prompt = null);
}