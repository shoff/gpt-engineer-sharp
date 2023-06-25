namespace GptEngineer.Client.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

public class ChatHubService
{
    private const string CHAT_HUB = "/chathub";
    private readonly NavigationManager navigationManager;
    
    public ChatHubService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }
    public HubConnection? GetHubConnection()
    {
       var hubConnection = new HubConnectionBuilder()
            .WithUrl(this.navigationManager.ToAbsoluteUri(CHAT_HUB))
        .Build();

        return hubConnection;
    }


}