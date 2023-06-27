namespace GptEngineer.Data.Stores;

using Core.Stores;
using GptEngineer.Data.Entities;
using MongoDB.Driver;

public class InputStore : IInputStore
{
    private readonly IInputDbContext dbContext;

    public InputStore(IInputDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key]
    {
        get
        {
            var filter = Builders<Input>.Filter.Eq("Key", key);
            var specification = this.dbContext.Inputs.Find(filter).FirstOrDefault();
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
            this.dbContext.Inputs.InsertOne(new Input { Role = key, Content = value });
        }
    }
}