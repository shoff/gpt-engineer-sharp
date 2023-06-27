namespace GptEngineer.Data;

public record SpecificationStoreOptions
{
    public string? DatabaseName { get; set; }
    public string? SpecificationCollectionName { get; set; }
}