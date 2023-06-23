namespace GptEngineer.Infrastructure;

using Core.Configuration;
using GptEngineer.Client.Services;
using GptEngineer.Core;
using GptEngineer.Core.Projects;
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
        // this might need to go on the API
        services.AddSingleton<IProjectFactory, ProjectFactory>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddTransient<ISteps, Steps>();
        services.AddTransient<IStepRunner, StepRunner>();
        services.AddTransient<IDataStores, DataStores>();
        services.AddTransient<IAI, AI>();
        return services;
    }
}