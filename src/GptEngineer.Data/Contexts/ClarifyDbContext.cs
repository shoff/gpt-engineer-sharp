namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public class ClarifyDbContext : IClarifyDbContext
{
    private readonly IMongoDatabase db;
    private readonly ClarifyOptions options;

    public ClarifyDbContext(IMongoClient client, ClarifyOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(options.DatabaseName)}");
        }

        db = client.GetDatabase(options.DatabaseName);
        this.options = options;
    }

    public IMongoCollection<Clarify> Clarifications =>
        db.GetCollection<Clarify>(options.ClarifyCollectionName);
}