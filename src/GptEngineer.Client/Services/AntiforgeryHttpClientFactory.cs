namespace GptEngineer.Client.Services;


using Microsoft.JSInterop;

public class AntiforgeryHttpClientFactory : IAntiforgeryHttpClientFactory
{
    private readonly ILogger<AntiforgeryHttpClientFactory> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IJSRuntime jSRuntime;

    public AntiforgeryHttpClientFactory(
        ILogger<AntiforgeryHttpClientFactory> logger,
        IHttpClientFactory httpClientFactory,
        IJSRuntime jSRuntime)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.jSRuntime = jSRuntime;
    }

    public async Task<HttpClient> CreateClientAsync(string clientName = AUTHORIZED_CLIENT_NAME)
    {
        var token = await this.jSRuntime.InvokeAsync<string>("getAntiForgeryToken");
        this.logger.LogDebug("AntiForgeryToken: {AntiForgeryToken}", token);

        var client = this.httpClientFactory.CreateClient(clientName);
        client.DefaultRequestHeaders.Add(HEADER_NAME, token);
        return client;
    }
}