namespace GptEngineer.Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNoUnauthorizedRedirect(this IApplicationBuilder applicationBuilder, params string[] segments)
    {
        applicationBuilder.Use(async (httpContext, func) =>
        {
            if (segments.Any(s => httpContext.Request.Path.StartsWithSegments(s)))
            {
                httpContext.Request.Headers[HeaderNames.XRequestedWith] = XML_HTTP_REQUEST_HEADER;
            }

            await func();
        });

        return applicationBuilder;
    }

    public static void SetupLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(LOG) ?? "mongodb://localhost/logs";

        builder.Host.UseSerilog((_, lc) => lc
#if DEBUG
            .MinimumLevel.Debug()
            .WriteTo.Console(theme: SystemConsoleTheme.Literate)
#endif
            .WriteTo.MongoDBBson(cfg =>
            {
                // custom MongoDb configuration
                var mongoDbSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                var mongoDbInstance = new MongoClient(mongoDbSettings)
                    .GetDatabase(LOG); // todo should be set from the application name probably
                // sink will use the IMongoDatabase instance provided
                cfg.SetMongoDatabase(mongoDbInstance);
                cfg.SetRollingInternal(Serilog.Sinks.MongoDB.RollingInterval.Month);
            }));
    }

    public static void SetupLogging(this WebAssemblyHostBuilder? builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(LOG) ?? "mongodb://localhost/logs";
        var levelSwitch = new LoggingLevelSwitch();
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
                .WriteTo.MongoDBBson(cfg =>
                {
                    // custom MongoDb configuration
                    var mongoDbSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                    var mongoDbInstance = new MongoClient(mongoDbSettings)
                        .GetDatabase(LOG); // todo should be set from the application name probably
                                           // sink will use the IMongoDatabase instance provided
                    cfg.SetMongoDatabase(mongoDbInstance);
                    cfg.SetRollingInternal(Serilog.Sinks.MongoDB.RollingInterval.Month);
                }).CreateLogger();

        builder?.Logging?.AddSerilog(dispose: true);
    }
}