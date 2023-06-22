using GptEngineer.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Serilog;
using GptEngineer.Infrastructure;
using Toolbelt.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// no easy way around this unless we create a custom bootstrap logger
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;

services.AddAntiforgery(options =>
{
    options.HeaderName = HEADER_NAME;
    options.Cookie.Name = COOKIE_NAME;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add services to the container.
OptionsServiceCollectionExtensions.ConfigureOptions(services, builder.Configuration);

builder.SetupLogging(builder.Configuration);

services.RegisterDependencies(builder.Configuration)
    .SetupSecurity()
    .AddControllersWithViews(options =>
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
IdentityModelEventSource.ShowPII = true;

services.AddRazorPages().AddMvcOptions(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //    .RequireAuthenticatedUser()
    //    .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();

// again no easy way to separate this!
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
#if DEBUG
app.UseCssLiveReload();
app.UseDeveloperExceptionPage();
app.UseWebAssemblyDebugging();
#else
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
#endif

app.UseSecurityHeaders(
    SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment(),
        configuration["OpenIDConnectSettings:Authority"]!));

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
#if !DEBUG
app.UseNoUnauthorizedRedirect("/api/v1");
app.UseAuthentication();
app.UseAuthorization();
#endif
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Run();
