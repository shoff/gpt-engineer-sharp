namespace GptEngineer.Data.Stores;

using Core.Stores;
using GptEngineer.Data.Contexts;
using GptEngineer.Data.Entities;
using MongoDB.Driver;

public class StepStore : IStepStore
{
    private readonly IStepDbContext dbContext;

    public StepStore(IStepDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key, string? role = null]
    {
        get
        {
            var filter = Builders<Step>.Filter.Eq(KEY, key);

            if (!string.IsNullOrEmpty(role) && role != "*")
            {
                filter &= Builders<Step>.Filter.Eq(ROLE, role);
            }

            var step = this.dbContext.Steps.Find(filter).FirstOrDefault();

            if (step != null)
            {
                return step.Content ?? string.Empty;
            }

            return string.Empty;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            this.dbContext.Steps.InsertOne(new Step { Key = key, Role = role, Content = value });
        }
    }
}