namespace GptEngineer.Data.Stores;

using Core.Stores;
using GptEngineer.Data.Contexts;
using GptEngineer.Data.Entities;
using MongoDB.Driver;

public class AIMemoryStore : IAIMemoryStore
{
    private readonly IAIMemoryDbContext dbContext;

    public AIMemoryStore(IAIMemoryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key]
    {
        get
        {
            var filter = Builders<AIMemory>.Filter.Eq("Key", key);
            var memory = this.dbContext.Memories.Find(filter).FirstOrDefault();
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
            this.dbContext.Memories.InsertOne(new AIMemory { Role = key, Content = value });
        }
    }
}