using GptEngineer.Data.Entities;
using MongoDB.Driver;

namespace GptEngineer.Data.Contexts;

public interface IPrePromptDbContext
{
    IMongoCollection<PrePrompt> PrePrompts { get; }
}