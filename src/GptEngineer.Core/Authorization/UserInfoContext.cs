namespace GptEngineer.Core.Authorization;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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