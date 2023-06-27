namespace GptEngineer.Infrastructure.Steps;

using System.Text.Json;
using Core;
using Core.StepDefinitions;
using Core.Stores;
using Data.Stores;

public class FixCode : Step, IFixCode
{
    private readonly IAI ai;
    private readonly IStepStore stepStore;
    private readonly IInputStore inputStore;
    private readonly IIdentityStore identityStore;
    private readonly IWorkspaceStore workspaceStore;

    public FixCode(
        IAI ai,
        IStepStore stepStore,
        IInputStore inputStore,
        IPrePromptStore prePromptStore,
        IIdentityStore identityStore,
        IWorkspaceStore workspaceStore)
        : base(prePromptStore)  
    {
        this.ai = ai;
        this.stepStore = stepStore;
        this.inputStore = inputStore;
        this.identityStore = identityStore;
        this.workspaceStore = workspaceStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        // TODO this is just fucking flat wrong
        // this should be a collection of step data
        var codeOutput = JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>
            (this.stepStore["GenCode"])?.Last()[CONTENT];

        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsRoleMessage(Role.System, this.SetupSysPrompt()),
            this.ai.AsRoleMessage(Role.User, $"Instructions: {this.inputStore[MAIN_PROMPT]}"),
            this.ai.AsRoleMessage(Role.User, codeOutput),
            this.ai.AsRoleMessage(Role.System, this.identityStore["fix_code"])
        };

        messages = await this.ai.NextAsync(messages, "Please fix any errors in the code above.");
        var runAsync = messages as Dictionary<string, string>[] ?? messages.ToArray();
        this.workspaceStore.ToFiles(runAsync.Last()[CONTENT]);
        return runAsync;
    }
}