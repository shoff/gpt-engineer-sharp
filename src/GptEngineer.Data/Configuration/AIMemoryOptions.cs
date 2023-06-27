namespace GptEngineer.Data.Configuration;

public record AIMemoryOptions
{
    public string? DatabaseName { get; set; }
    public string? MemoryCollectionName { get; set; }
}