using Blazored.LocalStorage;
using MudBlazor.Services;

namespace GptEngineer.Client;

using Core.Projects;

public static class Ioc
{
    public static WebAssemblyHostBuilder ConfigureOptions(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOptions();
        // builder.SetupLogging((IConfiguration)builder.Configuration);
        return builder;
    }

    public static WebAssemblyHostBuilder RegisterDependencies(this WebAssemblyHostBuilder builder)
    {

#if !DEBUG
            builder.Services.AddAuthorizationCore();
            builder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
            builder.Services.TryAddSingleton(sp => (HostAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
            builder.Services.AddTransient<AuthorizedHandler>();
#endif

        builder.Services.AddHttpClient(DEFAULT, client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(APPLICATION_JSON));
        });

        builder.Services.AddHttpClient(AUTHORIZED_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(APPLICATION_JSON));
        }).AddHttpMessageHandler<AuthorizedHandler>();



        builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DEFAULT));
        builder.Services.AddTransient<IAntiforgeryHttpClientFactory, AntiforgeryHttpClientFactory>();
        builder.Services.AddMudServices();
        builder.Services.AddMudBlazorDialog();
        builder.Services.AddBlazoredLocalStorage();
        return builder;
    }
}