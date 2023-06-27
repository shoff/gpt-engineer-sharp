namespace GptEngineer.Data.Configuration;

public record SpecificationStoreOptions
{
    public string? DatabaseName { get; set; }
    public string? SpecificationCollectionName { get; set; }
}