namespace GptEngineer.Data.Contexts;

using Entities;
using GptEngineer.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class StepDbContext : IStepDbContext
{
    private readonly IMongoDatabase db;
    private readonly StepOptions options;

    public StepDbContext(IMongoClient client, IOptions<StepOptions> options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(options.Value.DatabaseName)}");
        }

        db = client.GetDatabase(options.Value.DatabaseName);
        this.options = options.Value;
    }

    public IMongoCollection<Step> Steps =>
        db.GetCollection<Step>(options.StepsCollectionName);
}