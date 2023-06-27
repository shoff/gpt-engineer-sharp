namespace GptEngineer.Data;

using Entities;
using MongoDB.Driver;

public interface ISpecificationDbContext
{
    IMongoCollection<Specification> Specifications { get; }
}