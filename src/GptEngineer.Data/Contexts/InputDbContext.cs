namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public class InputDbContext : IInputDbContext
{
    private readonly IMongoDatabase db;
    private readonly InputOptions options;

    public InputDbContext(IMongoClient client, InputOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(InputOptions)}");
        }

        db = client.GetDatabase(options.DatabaseName);
        this.options = options;
    }

    public IMongoCollection<Input> Inputs =>
        db.GetCollection<Input>(options.InputCollectionName);
}