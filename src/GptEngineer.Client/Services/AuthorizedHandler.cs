namespace GptEngineer.Client.Services;

using System.Net;

public class AuthorizedHandler : DelegatingHandler
{
    private readonly ILogger<AuthorizedHandler> logger;
    private readonly HostAuthenticationStateProvider authenticationStateProvider;

    public AuthorizedHandler(
        ILogger<AuthorizedHandler> logger,
        HostAuthenticationStateProvider authenticationStateProvider)
    {
        this.logger = logger;
        this.authenticationStateProvider = authenticationStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authState = await this.authenticationStateProvider.GetAuthenticationStateAsync();

        this.logger.LogInformation("{UserName} authState: {Authenticated}", authState.User.Identity?.Name,
            authState.User.Identity is { IsAuthenticated: false });

        HttpResponseMessage responseMessage;

        if (authState.User.Identity is { IsAuthenticated: false })
        {
            this.logger.LogInformation("{User} is not authenticated for {Route}",
                authState.User.Identity.Name, request.RequestUri);

            // if user is not authenticated, immediately set response status to 401 Unauthorized
            responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        else
        {
            responseMessage = await base.SendAsync(request, cancellationToken);
        }

        if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            this.logger.LogInformation("{User} is not unauthorized for {Route}",
                authState.User.Identity?.Name, request.RequestUri);

            // if server returned 401 Unauthorized, redirect to login page
            this.authenticationStateProvider.SignIn();
        }

        return responseMessage;
    }
}
// orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth