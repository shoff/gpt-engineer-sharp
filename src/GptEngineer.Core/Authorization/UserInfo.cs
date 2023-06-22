namespace GptEngineer.Core.Authorization;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public record UserInfo
{
    public static readonly UserInfo anonymous = new();

    public bool IsAuthenticated { get; set; }
    public string NameClaimType { get; set; } = string.Empty;
    public string RoleClaimType { get; set; } = string.Empty;
    public ICollection<ClaimValue> Claims { get; set; } = new List<ClaimValue>();
}

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(UserInfo))]
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization,
    IgnoreReadOnlyFields = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = false)]
public partial class UserInfoContext : JsonSerializerContext
{
}

