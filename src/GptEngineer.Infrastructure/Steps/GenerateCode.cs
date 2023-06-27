namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.Stores;

public class GenerateCode : IStep
{
    private const string PHILOSOPHY = "philosophy";
    private const string GENERATE = "generate";
    private readonly IAI ai;
    private readonly IInputStore inputStore;
    private readonly IIdentityStore identityStore;
    private readonly IAIMemoryStore iaiMemoryStore;
    private readonly IWorkspaceStore workspaceStore;

    public GenerateCode(IAI ai,
        IInputStore inputStore,
        IIdentityStore identityStore,
        IAIMemoryStore iaiMemoryStore,
        IWorkspaceStore workspaceStore)
    {
        this.ai = ai;
        this.inputStore = inputStore;
        this.identityStore = identityStore;
        this.iaiMemoryStore = iaiMemoryStore;
        this.workspaceStore = workspaceStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        // this is all that AsUserRole does:
        // return new Dictionary<string, string> { { ROLE, USER }, { CONTENT, message } };

        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsUserRole($"Instructions: {this.inputStore[MAIN_PROMPT]}"),        // {this.dbs.Input[MAIN_PROMPT]}"),
            this.ai.AsUserRole($"Specification:\n\n{this.iaiMemoryStore[SPECIFICATION]}"), // {this.dbs.Memory[SPECIFICATION]}"),
            this.ai.AsUserRole($"Unit tests:\n\n{this.iaiMemoryStore[UNIT_TESTS]}")
        };

        messages = await this.ai.NextAsync(messages, this.identityStore[USE_QA]);
        var runAsync = messages as Dictionary<string, string>[] ?? messages.ToArray();
        this.workspaceStore.ToFiles(runAsync.First()[CONTENT]);
        return runAsync;
    }
    public string SetupSysPrompt()
    {
        return this.identityStore[GENERATE] + "\nUseful to know:\n" + this.identityStore[PHILOSOPHY];
    }
}