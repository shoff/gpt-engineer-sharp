namespace GptEngineer.Infrastructure.Steps;

using Core;
using Core.Stores;

public class Clarify : IStep
{
    private const string KEY = "qa";
    private readonly IAI ai;
    private readonly IClarifyStore clarifyStore;
    private readonly IIdentityStore identityStore;
    private readonly IInputStore inputStore;

    public Clarify(
        IAI ai,
        IClarifyStore clarifyStore,
        IIdentityStore identityStore,
        IInputStore inputStore)
    {
        this.ai = ai;
        this.clarifyStore = clarifyStore;
        this.identityStore = identityStore;
        this.inputStore = inputStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        //messages = [ai.fsystem(dbs.preprompts["qa"])]
        //user_input = get_prompt(dbs)
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.identityStore[KEY])
        };
        var user = this.inputStore[MAIN_PROMPT]; // user is the main prompt

        while (true)
        {
            var next = await this.ai.NextAsync(messages, user);

            if (messages.Last()[CONTENT].Trim().ToLower().StartsWith("no"))
            {
                break;
            }

            //Console.WriteLine();
            //user = Console.ReadLine();
            //Console.WriteLine();

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
}