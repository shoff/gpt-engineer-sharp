namespace GptEngineer.Core.Authorization;

public record ClaimValue
{
    public ClaimValue()
    {
    }

    public ClaimValue(string type, string value)
    {
        this.Type = type;
        this.Value = value;
    }

    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}