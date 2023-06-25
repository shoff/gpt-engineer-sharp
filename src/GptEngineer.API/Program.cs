namespace GptEngineer.API;

using GptEngineer.Infrastructure;
using GptEngineer.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Toolbelt.Extensions.DependencyInjection;

public static class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // no easy way around this unless we create a custom bootstrap logger
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

        var services = builder.Services;
        var configuration = builder.Configuration;
        var env = builder.Environment;

        services.ConfigureOptions(configuration);

        builder.SetupLogging(builder.Configuration);

        services.RegisterDependencies(builder.Configuration)
            .SetupSecurity()
            .AddControllersWithViews(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        IdentityModelEventSource.ShowPII = true;

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
#endif
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        app.UseSecurityHeaders(
            SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment(),
                configuration["OpenIDConnectSettings:Authority"]!));
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseNoUnauthorizedRedirect("/api/v1");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToPage("/_Host");
        app.Run();
    }
}