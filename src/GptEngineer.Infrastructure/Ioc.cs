namespace GptEngineer.Infrastructure;

using Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenAI.Extensions;

public static class Ioc
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Pages");
        services.Configure<GptOptions>(configuration.GetSection(GPT_OPTIONS));
        services.Configure<AIOptions>(configuration.GetSection(AI_OPTIONS));

        return services;
    }

    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddMediator();
        services.AddSingleton<IMongoClient>(_ =>
        {
            var connectionString = configuration.GetConnectionString(MONGO_DB);
            var mcs = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            return new MongoClient(mcs);
        });

        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException();
        });

        services.AddTransient<ISteps, Steps>();
        services.AddTransient<IStepRunner, StepRunner>();
        services.AddTransient<IDBs, DBs>();
        services.AddTransient<IAI, AI>();
        return services;
    }
}