namespace GptEngineer.API.Hubs;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        this.logger = logger;
    }

    public async Task SendMessage(string? user, string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            this.logger.LogError("Null, empty or whitespace message sent to ChatHub");
            return;
        }

        if (string.IsNullOrWhiteSpace(user))
        {
            user = this.Context.User?.Identity?.Name;
        }
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
