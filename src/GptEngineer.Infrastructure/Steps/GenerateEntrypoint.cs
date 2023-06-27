namespace GptEngineer.Infrastructure.Steps;

using Core.StepDefinitions;
using System.Text.RegularExpressions;
using Core;
using Core.Stores;
using StepDefinitions;

public class GenerateEntrypoint : IStep, IGenerateEntrypoint
{
    private readonly IAI ai;
    private readonly IWorkspaceStore workspaceStore;

    public GenerateEntrypoint(
        IAI ai,
        IWorkspaceStore workspaceStore)
    {
        this.ai = ai;
        this.workspaceStore = workspaceStore;
    }


    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        IEnumerable<Dictionary<string, string>> messages = await this.ai.Start(
            system: "You will get information about a codebase that is currently on disk in the current folder.\n" +
            "From this you will answer with code blocks that include all the necessary " +
            "Unix terminal commands to:\n" +
            "a) Install dependencies\n" +
            "b) Run all necessary parts of the codebase (in parallel if necessary).\n" +
            "Do not install globally. Do not use sudo.\n" +
            "Do not explain the code, just give the commands.",
            user: "Information about the codebase:\n\n" + this.workspaceStore["all_output.txt"]
        );
        // event?
        // Console.WriteLine();

        var regex = new Regex(@"```\S*\n(.+?)```", RegexOptions.Singleline); // huh?
        var runAsync = messages as Dictionary<string, string>[] ?? messages.ToArray();
        var matches = regex.Matches(runAsync.Last()[CONTENT]);
        this.workspaceStore["run.bat"] = string.Join("\n", matches.Select(match => match.Groups[1].Value));
        return runAsync;
    }
}