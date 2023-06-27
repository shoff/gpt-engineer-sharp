namespace GptEngineer.Data.Contexts;

using Core.Stores;
using Entities;
using GptEngineer.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class PrePromptDbContext : IPrePromptDbContext
{
    private readonly IMongoDatabase db;
    private readonly PrePromptOptions options;

    public PrePromptDbContext(IMongoClient client, IOptions<PrePromptOptions> options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(PrePrompt)}");
        }

        this.db = client.GetDatabase(options.Value.DatabaseName);
        this.options = options.Value;
    }

    public IMongoCollection<PrePrompt> PrePrompts =>
        this.db.GetCollection<PrePrompt>(this.options.PrePromptCollectionName);
}