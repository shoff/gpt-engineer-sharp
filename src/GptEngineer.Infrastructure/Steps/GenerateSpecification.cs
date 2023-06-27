namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.StepDefinitions;
using Core.Stores;
using Data.Stores;
using Microsoft.Extensions.Logging;

public class GenerateSpecification : Step, IStep, IGenerateSpecification
{
    private readonly ILogger<GenerateSpecification> logger;
    private readonly IAI ai;
    private readonly IInputStore inputStore;
    private readonly IIdentityStore identityStore;
    private readonly IAIMemoryStore iaiMemoryStore;

    public GenerateSpecification(
        ILogger<GenerateSpecification> logger,
        IAI ai,
        IPrePromptStore prePromptStore,
        IInputStore inputStore,
        IIdentityStore identityStore,
        IAIMemoryStore iaiMemoryStore) 
        : base(prePromptStore)
    {
        this.logger = logger;
        this.ai = ai;
        this.inputStore = inputStore;
        this.identityStore = identityStore;
        this.iaiMemoryStore = iaiMemoryStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        try
        {
            // Generate a spec from the main prompt + clarifications and save the
            // results to the workspace
            IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
            {
                this.ai.AsRoleMessage(Role.System, this.SetupSysPrompt()),
                this.ai.AsRoleMessage(Role.System, $"Instructions: {this.inputStore[MAIN_PROMPT]}")
            };

            // the call to next must persist or something
            messages = await this.ai.NextAsync(messages, this.identityStore[SPEC]);
            var genSpec = messages as Dictionary<string, string>[] ?? messages.ToArray();

            this.iaiMemoryStore[SPECIFICATION] = genSpec.Last()[CONTENT];
            return genSpec;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error in GenerateSpecification");
            throw;
        }
    }
}