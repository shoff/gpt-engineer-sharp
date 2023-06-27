namespace GptEngineer.Data.Contexts;

using Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class AIMemoryDbContext : IAIMemoryDbContext
{
    private readonly IMongoDatabase db;
    private readonly AIMemoryOptions options;

    public AIMemoryDbContext(
        IMongoClient client, 
        IOptions<AIMemoryOptions> options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(AIMemory)}");
        }

        db = client.GetDatabase(options.Value.DatabaseName);
        this.options = options.Value;
    }

    public IMongoCollection<AIMemory> Memories =>
        db.GetCollection<AIMemory>(options.MemoryCollectionName);
}