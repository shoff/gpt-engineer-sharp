namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public interface IClarifyDbContext
{
    IMongoCollection<Clarify> Clarifications { get; }
}