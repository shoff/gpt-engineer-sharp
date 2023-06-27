namespace GptEngineer.Data;

using Entities;
using MongoDB.Driver;

public interface IInputDbContext
{
    IMongoCollection<Input> Inputs { get; }
}