namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public interface IInputDbContext
{
    IMongoCollection<Input> Inputs { get; }
}