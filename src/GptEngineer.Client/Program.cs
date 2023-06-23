
using Microsoft.AspNetCore.Components.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>(ROOT_COMPONENT);
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder
    .ConfigureOptions()
    .RegisterDependencies()
    .Build()
    .RunAsync();