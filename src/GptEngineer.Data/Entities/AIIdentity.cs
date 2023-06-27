namespace GptEngineer.Data.Entities;

using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class AIIdentity
{
    public string? Role { get; set; }
    public string? Content { get; set; }
}