namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.Stores;

public class GenerateSpecification : IStep
{
    private const string PHILOSOPHY = "philosophy";
    private const string GENERATE = "generate";
    private readonly IAI ai;
    private readonly IInputStore inputStore;
    private readonly IIdentityStore identityStore;
    private readonly IAIMemoryStore iaiMemoryStore;

    public GenerateSpecification(IAI ai,
        IInputStore inputStore,
        IIdentityStore identityStore,
        IAIMemoryStore iaiMemoryStore)
    {
        this.ai = ai;
        this.inputStore = inputStore;
        this.identityStore = identityStore;
        this.iaiMemoryStore = iaiMemoryStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        // Generate a spec from the main prompt + clarifications and save the
        // results to the workspace
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsSystemRole($"Instructions: {this.inputStore[MAIN_PROMPT]}")
        };

        // the call to next must persist or something
        messages = await this.ai.NextAsync(messages, this.identityStore[SPEC]);
        var genSpec = messages as Dictionary<string, string>[] ?? messages.ToArray();
        
        this.iaiMemoryStore[SPECIFICATION] = genSpec.Last()[CONTENT];
        return genSpec;
    }

    private string SetupSysPrompt()
    {
        return this.identityStore[GENERATE] + "\nUseful to know:\n" + this.identityStore[PHILOSOPHY];
    }

}