namespace GptEngineer.Data;

using Core.Stores;
using Entities;
using MongoDB.Driver;

public class SpecificationStore : ISpecificationStore
{
    private readonly ISpecificationDbContext dbContext;

    public SpecificationStore(ISpecificationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public string this[string key]
    {
        get
        {
            var filter = Builders<Specification>.Filter.Eq("Key", key);
            var specification = this.dbContext.Specifications.Find(filter).FirstOrDefault();
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
            this.dbContext.Specifications.InsertOne(new Specification { Role = key, Content = value });
        }
    }
}