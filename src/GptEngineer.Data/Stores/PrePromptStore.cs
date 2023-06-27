namespace GptEngineer.Data.Stores;

using Contexts;
using Entities;
using MongoDB.Driver;

public class PrePromptStore : IPrePromptStore
{
    private readonly IPrePromptDbContext dbContext;

    public PrePromptStore(IPrePromptDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key]
    {
        get
        {
            var filter = Builders<PrePrompt>.Filter.Eq("Key", key);
            var specification = this.dbContext.PrePrompts.Find(filter).FirstOrDefault();
            if (specification != null)
            {
                return specification.Content ?? string.Empty;
            }

            return string.Empty;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            this.dbContext.PrePrompts.InsertOne(new PrePrompt { Role = key, Content = value });
        }
    }
}