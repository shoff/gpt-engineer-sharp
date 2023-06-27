namespace GptEngineer.Data.Entities;

using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Step   
{
    public string? Key { get; set; }
    public string? Role { get; set; }
    public string? Content { get; set; }
}