using Microsoft.AspNetCore.SignalR;

namespace GptEngineer.API.Hubs;

public class AIHub : Hub
{
    private readonly ILogger<AIHub> logger;

    public AIHub(ILogger<AIHub> logger)
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