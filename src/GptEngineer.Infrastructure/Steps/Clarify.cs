namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.StepDefinitions;
using Core.Stores;
using Microsoft.Extensions.Logging;

public class Clarify : IStep, IClarify
{
    private const string KEY = "qa";
    private readonly IAI ai;
    private readonly ILogger<Clarify> logger;
    private readonly IStepStore stepStore;
    private readonly IIdentityStore identityStore;
    private readonly IInputStore inputStore;

    public Clarify(
        IAI ai,
        ILogger<Clarify> logger,
        IStepStore stepStore,
        IIdentityStore identityStore,
        IInputStore inputStore)
    {
        this.ai = ai;
        this.logger = logger;
        this.stepStore = stepStore;
        this.identityStore = identityStore;
        this.inputStore = inputStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        try
        {
            var messages = new List<Dictionary<string, string>>
            {
                this.ai.AsRoleMessage(Role.System, this.identityStore[KEY])
            };
            var user = this.inputStore[MAIN_PROMPT]; // user is the main prompt

            while (true)
            {
                // TODO this is absolutely fucking wrong
                var next = await this.ai.NextAsync(messages, user);

                if (messages.Last()[CONTENT].Trim().ToLower().StartsWith("no"))
                {
                    break;
                }
                
                if (string.IsNullOrEmpty(user) || user == "q")
                {
                    break;
                }

                user += (
                    "\n\n"
                    + "Is anything else unclear? If yes, only answer in the form:\n"
                    + "{remaining unclear areas} remaining questions.\n"
                    + "{Next question}\n"
                    + "If everything is sufficiently clear, only answer `no`."
                );
            }

            return messages;
        }
        catch (Exception e)
        {
            this.logger.LogError("{Message}", e.Message);
            throw;
        }
    }
}