namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public interface IAIMemoryDbContext
{
    IMongoCollection<AIMemory> Memories { get; }
}