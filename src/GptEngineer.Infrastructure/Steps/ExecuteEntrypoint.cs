namespace GptEngineer.Infrastructure.Steps;

using Core.StepDefinitions;
using CliWrap;
using Core.Stores;
using Data.Stores;

public class ExecuteEntrypoint : IStep, IExecuteEntrypoint
{
    private readonly IPrePromptStore prePromptStore;
    private readonly IWorkspaceStore workspaceStore;

    public ExecuteEntrypoint(
        IPrePromptStore prePromptStore,
        IWorkspaceStore workspaceStore)
    {
        this.prePromptStore = prePromptStore;
        this.workspaceStore = workspaceStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        // TODO this is hackish and should allow for user input
        // this seems incredibly fragile
        var command = this.workspaceStore["run.bat"];

        // TODO should be configurable
        _=await Cli.Wrap("C:\\tools\\Cmder\\cmder.exe")
            .WithArguments("run.bat")
            .WithWorkingDirectory(this.workspaceStore["path"])
            .ExecuteAsync();

        return new List<Dictionary<string, string>>();
    }
}