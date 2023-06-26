namespace GptEngineer.API.Hubs;

using GptEngineer.Core.Projects;
using GptEngineer.Infrastructure.Extensions;
using System.Text.Json;
using GptEngineer.Core;
using Microsoft.Extensions.Options;
using GptEngineer.Core.Configuration;
using Microsoft.AspNetCore.SignalR;
using OpenAI.Managers;
using OpenAI;

public class ChatHub : Hub
{
    private const string PROJECT_LOADED = "ProjectLoaded";
    private const string WORKSPACE = "workspace";
    private const string MEMORY = "memory";

    public static OpenAIService AIService { get; private set; } = new(new OpenAiOptions()
    {
        ApiKey = Environment.GetEnvironmentVariable(OPENAI_API_KEY)
                 ?? throw new InvalidOperationException("You must add an environment variable called 'OPENAI_API_KEY' and set to your OpenAI API Key see here: https://thiswebsite.com/openai-instructions")
    });

    private readonly ILogger<ChatHub> logger;
    private readonly IOptions<AIOptions> options;
    private readonly IDataStores dataStores;
    private readonly IStepRunner stepRunner;
    private readonly IProjectFactory projectFactory;
    private readonly IAI ai;
    private Project? project;

    public ChatHub(
        ILogger<ChatHub> logger,
        IOptions<AIOptions> options,
        IAI ai,
        IDataStores dataStores,
        IStepRunner stepRunner,
        IProjectFactory projectFactory)
    {
        options?.Validate();
        this.options = options!;
        this.logger = logger;
        this.dataStores = dataStores;
        this.stepRunner = stepRunner;
        this.projectFactory = projectFactory;
        this.ai = ai;
        
        AIService.SetDefaultModelId(options.Value.Model);

        if (options.Value.DeleteExisting)
        {
            try
            {
                // Delete files and subdirectories in paths
                var inputPath = Path.GetFullPath(options.Value.ProjectPath);
                Directory.Delete(Path.Combine(inputPath, options.Value.RunPrefix + MEMORY), true);
                Directory.Delete(Path.Combine(inputPath, options.Value.RunPrefix + WORKSPACE), true);
            }
            catch (Exception ex)
            {
                this.logger.LogError("{Message}", ex.Message);
            }
        }
    }

    public async Task StartProject(string projectName)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectName, nameof(projectName));
        this.project = await this.projectFactory.CreateProjectAsync(projectName);
        var projectInformation = new ProjectInformation(this.project.Name,
            this.project.HasWorkspace ? this.project.Workspace.FileList.Count : 0);

        var message = $"Successfully loaded {projectInformation}.";
        await this.Clients.Caller.SendAsync(PROJECT_LOADED, message);
    }

    public async Task SendMessage(string? user, string? message)
    {
        foreach (var step in this.stepRunner.Default)
        {
            try
            {
                var messages = await step();
                this.dataStores.Logs[step.GetType().Name] = JsonSerializer.Serialize(messages);
            }
            catch (Exception ex)
            {
                this.logger.LogError("{Message}", ex.Message);
            }
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            this.logger.LogError("Null, empty or whitespace message sent to ChatHub");
            return;
        }

        if (string.IsNullOrWhiteSpace(user))
        {
            user = this.Context.User?.Identity?.Name;
        }

        await this.Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}