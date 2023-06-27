namespace GptEngineer.Data.Contexts;

using Entities;
using GptEngineer.Data.Configuration;
using MongoDB.Driver;

public sealed class SpecificationDbContext : ISpecificationDbContext
{
    private readonly IMongoDatabase db;
    private readonly SpecificationStoreOptions options;

    public SpecificationDbContext(IMongoClient client, SpecificationStoreOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(SpecificationStoreOptions)}");
        }
        db = client.GetDatabase(options.DatabaseName);
        this.options = options;
    }

    public IMongoCollection<Specification> Specifications => db.GetCollection<Specification>(options.SpecificationCollectionName);
}
