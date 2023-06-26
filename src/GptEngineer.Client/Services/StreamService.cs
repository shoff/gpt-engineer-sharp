namespace GptEngineer.Client.Services;
public class StreamService
{
    private readonly HttpClient httpClient;

    public StreamService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async IAsyncEnumerable<int> GetStream()
    {
        var stream = await this.httpClient.GetStreamAsync("api/v1/ai");
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            yield return int.Parse(line);
        }
    }
}