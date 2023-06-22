using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(ClaimValue))]
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization,
    IgnoreReadOnlyFields = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = false)]
public partial class ClaimValueContext : JsonSerializerContext
{
}