namespace GptEngineer.Data;

using Entities;
using MongoDB.Driver;

public interface IAIMemoryDbContext
{
    IMongoCollection<AIMemory> Memories { get; }
}