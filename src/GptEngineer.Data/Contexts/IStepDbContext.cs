namespace GptEngineer.Data.Contexts;

using Entities;
using MongoDB.Driver;

public interface IStepDbContext
{
    IMongoCollection<Step> Steps { get; }
}