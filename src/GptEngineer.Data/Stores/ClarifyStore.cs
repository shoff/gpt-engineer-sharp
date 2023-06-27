namespace GptEngineer.Data.Stores;

using Core.Stores;
using GptEngineer.Data.Contexts;
using GptEngineer.Data.Entities;
using MongoDB.Driver;

public class ClarifyStore : IClarifyStore
{
    private readonly IClarifyDbContext dbContext;

    public ClarifyStore(IClarifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key]
    {
        get
        {
            var filter = Builders<Clarify>.Filter.Eq("Key", key);
            var memory = this.dbContext.Clarifications.Find(filter).FirstOrDefault();
            if (memory != null)
            {
                return memory.Content ?? string.Empty;
            }

            return string.Empty;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            this.dbContext.Clarifications.InsertOne(new Clarify { Role = key, Content = value });
        }
    }
}