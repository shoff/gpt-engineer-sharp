namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public interface ISpecificationDbContext
{
    IMongoCollection<Specification> Specifications { get; }
}