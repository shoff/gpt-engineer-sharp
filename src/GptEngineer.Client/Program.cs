
using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddOptions();
// builder.SetupLogging((IConfiguration)builder.Configuration);
builder.Services.AddAuthorizationCore();
builder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
builder.Services.TryAddSingleton(sp => (HostAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddTransient<AuthorizedHandler>();

builder.RootComponents.Add<App>(ROOT_COMPONENT);
builder.RootComponents.Add<HeadOutlet>("head::after");

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

await builder
    .Build()
    .RunAsync();