namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public class AIMemoryDbContext : IAIMemoryDbContext
{
    private readonly IMongoDatabase db;
    private readonly AIMemoryOptions options;

    public AIMemoryDbContext(IMongoClient client, AIMemoryOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(AIMemory)}");
        }

        db = client.GetDatabase(options.DatabaseName);
        this.options = options;
    }

    public IMongoCollection<AIMemory> Memories =>
        db.GetCollection<AIMemory>(options.MemoryCollectionName);
}