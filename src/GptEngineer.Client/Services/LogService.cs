using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace GptEngineer.Client.Services;

public class LogService : ILogService
{
    private readonly HttpClient client;

    public LogService(HttpClient client)
    {
        this.client = client;
    }

    public async Task LogError(string message)
    {
        var errorMessageContent = new StringContent(
            JsonSerializer.Serialize(message),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        _ = await this.client
            .PostAsync("api/v1/log/error", errorMessageContent);

    }
}