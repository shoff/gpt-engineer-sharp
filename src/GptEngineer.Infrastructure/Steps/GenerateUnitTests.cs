namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.StepDefinitions;
using Core.Stores;
using StepDefinitions;

public class GenerateUnitTests : IStep, IGenerateUnitTests
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
        IWorkspaceStore workspaceStore)
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
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.inputStore[MAIN_PROMPT]}"), this.ai.AsUserRole($"Specification:\n\n{this.memoryStore[SPECIFICATION]}")
        };

        messages = await this.ai.NextAsync(messages, this.identityStore[UNIT_TESTS]);
        var runAsync = messages as Dictionary<string, string>[] ?? messages.ToArray();
        this.memoryStore[UNIT_TESTS] = runAsync.Last()[CONTENT];
        this.workspaceStore.ToFiles(this.memoryStore[UNIT_TESTS]);
        return runAsync;
    }


    private string SetupSysPrompt()
    {
        return this.identityStore[GENERATE] + "\nUseful to know:\n" + this.identityStore[PHILOSOPHY];
    }
}