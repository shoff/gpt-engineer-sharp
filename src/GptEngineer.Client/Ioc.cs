namespace GptEngineer.Client;
using Blazored.LocalStorage;
using MudBlazor.Services;

public static class Ioc
{
    public static WebAssemblyHostBuilder ConfigureOptions(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOptions();
        return builder;
    }

    public static WebAssemblyHostBuilder RegisterDependencies(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<GlobalStateService>();
        builder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
        builder.Services.TryAddSingleton(sp => (HostAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
        builder.Services.AddTransient<AuthorizedHandler>();
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

        builder.Services.AddHttpClient<ILogService, LogService>();
        builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DEFAULT));
        builder.Services.AddTransient<IAntiforgeryHttpClientFactory, AntiforgeryHttpClientFactory>();
        builder.Services.AddMudServices();
        builder.Services.AddMudBlazorDialog();
        builder.Services.AddBlazoredLocalStorage();
        return builder;
    }

}