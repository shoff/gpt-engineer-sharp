namespace GptEngineer.Infrastructure.Steps;

using Core.StepDefinitions;
using System.Diagnostics;
using Core.Stores;
using StepDefinitions;

public class ExecuteEntrypoint : IStep, IExecuteEntrypoint
{
    private readonly IWorkspaceStore workspaceStore;

    public ExecuteEntrypoint(
        IWorkspaceStore workspaceStore)
    {
        this.workspaceStore = workspaceStore;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> RunAsync()
    {
        var command = this.workspaceStore["run.bat"];

        //Console.WriteLine("Do you want to execute this code?");
        //Console.WriteLine();
        //Console.WriteLine(command);
        //Console.WriteLine();
        //Console.WriteLine("If yes, press enter. Otherwise, type \"no\"");
        //Console.WriteLine();

        //if (!string.IsNullOrEmpty(Console.ReadLine()))
        //{
        //    Console.WriteLine("Ok, not executing the code.");
        //    return new IEnumerable<Dictionary<string, string>>();
        //}

        //Console.WriteLine("Executing the code...");
        //Console.WriteLine();

        // TODO should be configurable
        await Process.Start(new ProcessStartInfo
        {
            FileName = "C:\\tools\\Cmder\\cmder.exe",
            Arguments = "run.bat",
            WorkingDirectory = this.workspaceStore["path"]
        })?.WaitForExitAsync()!;

        return new List<Dictionary<string, string>>();
    }
}