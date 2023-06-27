namespace GptEngineer.Data.Configuration;

public record PrePromptOptions
{
    public string? DatabaseName { get; set; }
    public string? PrePromptCollectionName { get; set; }
}