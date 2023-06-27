namespace GptEngineer.Data.Configuration;

public record ReviewOptions
{
    public string? DatabaseName { get; set; }
    public string? ReviewCollectionName { get; set; }
}