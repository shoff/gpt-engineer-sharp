namespace GptEngineer.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

public static class AuthenticationExtensions
{
    public static IServiceCollection SetupSecurity(this IServiceCollection services)
    {
        // TODO Grab from config like we should
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddCookie() //CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = "code";
                    options.Authority = "https://localhost:7001";
                    options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.ClientId = "budohub-oidc";
                    options.ClientSecret = "secret";
                    // When set to code, the middleware will use PKCE protection
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name"
                    };
                    // Save the tokens we receive from the IDP
                    options.SaveTokens = true;

                    // It's recommended to always get claims from the 
                    // UserInfoEndpoint during the flow. 
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnAccessDenied = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/NotFound");
                            return Task.CompletedTask;
                        }
                    };
                });

        return services;

        //builder.Services.AddOidcAuthentication(options =>
        //{
        //    options.ProviderOptions.Authority = "https://localhost:7001";
        //    options.ProviderOptions.ClientId = "budohub-oidc";
        //    options.ProviderOptions.DefaultScopes.Add("openid");
        //    options.ProviderOptions.DefaultScopes.Add("profile");
        //    options.ProviderOptions.PostLogoutRedirectUri = "/";
        //    options.ProviderOptions.ResponseType = "code";
        //});
    }
}