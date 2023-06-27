namespace GptEngineer.Data.Entities;

using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Review : IReview
{
    public bool Ran { get; set; }
    public bool Perfect { get; set; }
    public bool Works { get; set; }
    public string? Comments { get; set; }
    public string? Raw { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
}