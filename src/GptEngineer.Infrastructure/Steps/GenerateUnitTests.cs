namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.StepDefinitions;
using Core.Stores;
using Data.Stores;

public class GenerateUnitTests : Step, IStep, IGenerateUnitTests
{
    private readonly IWorkspaceStore workspaceStore;
    private readonly IAI ai;
    private readonly IInputStore inputStore;
    private readonly IIdentityStore identityStore;
    private readonly IAIMemoryStore memoryStore;

    public GenerateUnitTests(IAI ai,
        IInputStore inputStore,
        IIdentityStore identityStore,
        IAIMemoryStore memoryStore,
        IPrePromptStore prePromptStore,
        IWorkspaceStore workspaceStore)
        : base(prePromptStore)
    {
        this.ai = ai;
        this.inputStore = inputStore;
        this.identityStore = identityStore;
        this.memoryStore = memoryStore;
        this.workspaceStore = workspaceStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsRoleMessage(Role.System, this.SetupSysPrompt()), this.ai.AsRoleMessage(Role.User, $"Instructions: {this.inputStore[MAIN_PROMPT]}"), this.ai.AsRoleMessage(Role.User, $"Specification:\n\n{this.memoryStore[SPECIFICATION]}")
        };

        messages = await this.ai.NextAsync(messages, this.identityStore[UNIT_TESTS]);
        var runAsync = messages as Dictionary<string, string>[] ?? messages.ToArray();
        this.memoryStore[UNIT_TESTS] = runAsync.Last()[CONTENT];
        this.workspaceStore.ToFiles(this.memoryStore[UNIT_TESTS]);
        return runAsync;
    }
}