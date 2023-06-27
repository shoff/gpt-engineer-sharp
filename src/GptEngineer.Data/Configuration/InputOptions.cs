namespace GptEngineer.Data.Configuration;

public record InputOptions
{
    public string? DatabaseName { get; set; }
    public string? InputCollectionName { get; set; }
}