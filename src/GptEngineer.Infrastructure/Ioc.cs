namespace GptEngineer.Infrastructure;


using GptEngineer.Infrastructure.Projects;
using GptEngineer.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Core.Configuration;
using Core.StepDefinitions;
using Data.Stores;
using Extensions;
using GptEngineer.Core;
using GptEngineer.Core.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenAI.Extensions;
using StackExchange.Redis;
using GptEngineer.Core.Stores;
using GptEngineer.Data.Contexts;
using GptEngineer.Infrastructure.Steps;
using GptEngineer.Data.Configuration;

public static class Ioc
{
    private const string MONGO_CONNECTION_NAME = "MongoDB";
    
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(); services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Pages");
        services.Configure<GptOptions>(configuration.GetSection(GPT_OPTIONS));
        services.Configure<WorkspaceOptions>(configuration.GetSection(WORKSPACE_OPTIONS));
        services.Configure<AIOptions>(configuration.GetSection(AI_OPTIONS));
        services.Configure<RedisOptions>(configuration.GetSection(REDIS_OPTIONS));
        services.Configure<AIMemoryOptions>(configuration.GetSection(AI_MEMORY_OPTIONS));
        services.Configure<InputOptions>(configuration.GetSection(INPUT_OPTIONS));
        services.Configure<IdentityOptions>(configuration.GetSection(IDENTITY_OPTIONS));
        services.Configure<StepOptions>(configuration.GetSection(STEP_OPTIONS));
        services.Configure<SpecificationStoreOptions>(configuration.GetSection(SPECIFICATION_STORE_OPTIONS));
        services.Configure<PrePromptOptions>(configuration.GetSection(PRE_PROMPT_OPTIONS));
        services.Configure<ReviewOptions>(configuration.GetSection(REVIEW_OPTIONS));
        return services;
    }

    public static IServiceCollection RegisterDependencies(
        this IServiceCollection services, IConfiguration configuration)
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
            settings.ApiKey = Environment.GetEnvironmentVariable(OPENAI_API_KEY)
                ?? throw new InvalidOperationException();
        });

        services.AddAntiforgery(options =>
        { 
            options.HeaderName = HEADER_NAME;
            options.Cookie.Name = COOKIE_NAME;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddRazorPages().AddMvcOptions(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });
        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { APPLICATION_OCTET_STREAM });
        });

        var connectionString = configuration.GetConnectionString(MONGO_CONNECTION_NAME);
        services.AddSingleton<IMongoClient>(_ =>
        {
            var mcs = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            return new MongoClient(mcs);
        });

        var redisOptions = configuration.RedisOptions();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions.Configuration;
            options.InstanceName = redisOptions.InstanceName;
            options.ConfigurationOptions = new ConfigurationOptions
            {
                AllowAdmin = redisOptions.ConfigurationOptions.AllowAdmin,
                AbortOnConnectFail = false,
                AsyncTimeout = redisOptions.ConfigurationOptions.AsyncTimeout,
                ChannelPrefix = redisOptions.ConfigurationOptions.ChannelPrefix,
                ClientName = redisOptions.ConfigurationOptions.ClientName,
                CommandMap = CommandMap.Create(redisOptions.ConfigurationOptions.Commands, available: false),
                ConfigCheckSeconds = redisOptions.ConfigurationOptions.ConfigCheckSeconds,
                ConfigurationChannel = redisOptions.ConfigurationOptions.ConfigurationChannel,
                ConnectRetry = redisOptions.ConfigurationOptions.ConnectRetry,
                DefaultDatabase = redisOptions.ConfigurationOptions.DefaultDatabase,
                HighPrioritySocketThreads = redisOptions.ConfigurationOptions.HighPrioritySocketThreads,
                KeepAlive = redisOptions.ConfigurationOptions.KeepAlive,
                Password = redisOptions.ConfigurationOptions.Password,
                Proxy = redisOptions.ConfigurationOptions.Proxy > -1 ? (Proxy)redisOptions.ConfigurationOptions.Proxy : Proxy.None,
                ServiceName = redisOptions.ConfigurationOptions.ServiceName,
                Ssl = redisOptions.ConfigurationOptions.UseSsl,
                SslHost = redisOptions.ConfigurationOptions.SslHost,
                SyncTimeout = redisOptions.ConfigurationOptions.SyncTimeout,
                TieBreaker = redisOptions.ConfigurationOptions.TieBreaker,
                SslProtocols = redisOptions.ConfigurationOptions.SslProtocols
            };

            foreach (var endpoint in redisOptions.ConfigurationOptions.Endpoints)
            {
                options.ConfigurationOptions.EndPoints.Add(endpoint.Host, endpoint.Port);
            }
        });

        // this might need to go on the API
        
        services.AddTransient<IStepDbContext, StepDbContext>();
        services.AddTransient<IInputDbContext,  InputDbContext>();
        services.AddTransient<IAIMemoryDbContext, AIMemoryDbContext>();
        services.AddTransient<IPrePromptDbContext, PrePromptDbContext>();
        services.AddTransient<IReviewDbContext, ReviewDbContext>();

        // steps
        services.AddTransient<IGenerateSpecification, GenerateSpecification>();
        services.AddTransient<IGenerateCode, GenerateCode>();
        services.AddTransient<IGenerateUnitTests, GenerateUnitTests>();
        services.AddTransient<IClarify, Clarify>();
        services.AddTransient<IGenerateEntrypoint, GenerateEntrypoint>();
        services.AddTransient<IExecuteEntrypoint, ExecuteEntrypoint>();
        services.AddSingleton<IReviewStore, ReviewStore>();
        services.AddSingleton<IPrePromptStore, PrePromptStore>();
        services.AddScoped<IReviewService, ReviewService>();

        services.AddSingleton<IStepStore, StepStore>();
        services.AddSingleton<ISpecificationStore, SpecificationStore>();
        services.AddSingleton<IWorkspaceStore, WorkspaceStore>();
        services.AddSingleton<IInputStore, InputStore>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<IAIMemoryStore, AIMemoryStore>();
        services.AddSingleton<IIdentityStore, IdentityStore>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<IProjectFactory, ProjectFactory>();
        services.AddScoped<ISteps, Core.Steps>();
        services.AddScoped<IStepRunner, StepRunner>();
        services.AddScoped<IDataStores, DataStores>();
        services.AddScoped<IAI, AI>();
        return services;
    }
}