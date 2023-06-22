namespace GptEngineer.Client.Services;

using System.Net.Http.Json;
using System.Security.Claims;
using Core.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public class HostAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly TimeSpan userCacheRefreshInterval = TimeSpan.FromSeconds(60);
    private readonly NavigationManager navigation;
    private readonly HttpClient client;
    private readonly ILogger<HostAuthenticationStateProvider> logger;
    private DateTimeOffset userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
    private ClaimsPrincipal cachedUser = new(new ClaimsIdentity());

    public HostAuthenticationStateProvider(NavigationManager navigation, HttpClient client, ILogger<HostAuthenticationStateProvider> logger)
    {
        this.navigation = navigation;
        this.client = client;
        this.logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() => new(await this.GetUser(useCache: true));

    public void SignIn(string? customReturnUrl = null)
    {
        var returnUrl = customReturnUrl != null ? this.navigation.ToAbsoluteUri(customReturnUrl).ToString() : null;
        var encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? this.navigation.Uri);
        var logInUrl = this.navigation.ToAbsoluteUri($"{LOG_IN_PATH}?returnUrl={encodedReturnUrl}");
        this.navigation.NavigateTo(logInUrl.ToString(), true);
    }

    private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
    {
        var now = DateTimeOffset.Now;
        if (useCache && now < this.userLastCheck + userCacheRefreshInterval)
        {
            this.logger.LogDebug("Taking user from cache");
            return this.cachedUser;
        }

        this.logger.LogDebug("Fetching user");
        this.cachedUser = await this.FetchUser();
        this.userLastCheck = now;

        return this.cachedUser;
    }

    private async Task<ClaimsPrincipal> FetchUser()
    {
        UserInfo? user = null;

        try
        {
            this.logger.LogInformation("{clientBaseAddress}", this.client.BaseAddress?.ToString());
            user = await this.client.GetFromJsonAsync<UserInfo>("api/v1/User");
        }
        catch (Exception exc)
        {
            this.logger.LogWarning(exc, "Fetching user failed.");
        }

        if (user is not { IsAuthenticated: true })
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var identity = new ClaimsIdentity(
            nameof(HostAuthenticationStateProvider),
            user.NameClaimType,
            user.RoleClaimType);

        if (user.Claims != null!)
        {
            identity.AddClaims(user.Claims.Select(c => new Claim(c.Type, c.Value)));
        }

        return new ClaimsPrincipal(identity);
    }
}