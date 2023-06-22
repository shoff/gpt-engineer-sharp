namespace GptEngineer.Client.Services;

public interface IAntiforgeryHttpClientFactory
{
    Task<HttpClient> CreateClientAsync(string clientName = AUTHORIZED_CLIENT_NAME);
}