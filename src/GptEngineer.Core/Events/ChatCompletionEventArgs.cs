namespace GptEngineer.Core.Events;

public class ChatCompletionEventArgs : EventArgs
{
    public string? Message { get; set; }
}