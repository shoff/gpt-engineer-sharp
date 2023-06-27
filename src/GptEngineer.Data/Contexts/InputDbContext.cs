namespace GptEngineer.Data.Contexts;

using Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class InputDbContext : IInputDbContext
{
    private readonly IMongoDatabase db;
    private readonly InputOptions options;

    public InputDbContext(IMongoClient client, IOptions<InputOptions> options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(InputOptions)}");
        }

        db = client.GetDatabase(options.Value.DatabaseName);
        this.options = options.Value;
    }

    public IMongoCollection<Input> Inputs =>
        db.GetCollection<Input>(options.InputCollectionName);
}