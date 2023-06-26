namespace GptEngineer.Core.Authorization;

public record UserInfo
{
    public static readonly UserInfo anonymous = new();

    public bool IsAuthenticated { get; set; }
    public string NameClaimType { get; set; } = string.Empty;
    public string RoleClaimType { get; set; } = string.Empty;
    public ICollection<ClaimValue> Claims { get; set; } = new List<ClaimValue>();
}