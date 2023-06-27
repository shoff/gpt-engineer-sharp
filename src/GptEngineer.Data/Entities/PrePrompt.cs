using MongoDB.Bson.Serialization.Attributes;

namespace GptEngineer.Data.Entities;

[BsonIgnoreExtraElements]
public class PrePrompt
{
    public string? Role { get; set; }
    public string? Content { get; set; }
}