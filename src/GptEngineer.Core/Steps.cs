// ReSharper disable ParameterHidesMember
namespace GptEngineer.Core;

using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Steps : ISteps
{
    private readonly IDataStores dbs;
    private readonly IAI ai;
    private static readonly Regex regex = new(@"(\S+?)\n```\S+\n(.+?)```", RegexOptions.Compiled);

    public Steps(IAI ai, IDataStores dbs)
    {
        this.ai = ai;
        this.dbs = dbs;
    }

    public string SetupSysPrompt()
    {
        return this.dbs.Identity["generate"] + "\nUseful to know:\n" + this.dbs.Identity["philosophy"];
    }

    public async Task<List<Dictionary<string, string>>> SimpleGen()
    {
        // Run the AI on the main prompt and save the results
        List<Dictionary<string, string>> messages = await this.ai.Start(this.SetupSysPrompt(), this.dbs.Input["main_prompt"]);
        this.ToFiles(messages.Last()["content"], this.dbs.Workspace);
        return messages;
    }
    
    public async Task<List<Dictionary<string, string>>> Clarify()
    {
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.dbs.Identity["qa"])
        };
        var user = this.dbs.Input["main_prompt"]; // user is the main prompt

        while (true)
        {
            var next = await this.ai.NextAsync(messages, user);
            if (messages.Last()["content"].Trim().ToLower().StartsWith("no"))
            {
                break;
            }

            Console.WriteLine();
            user = Console.ReadLine();
            Console.WriteLine();

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

        Console.WriteLine();
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> GenSpec()
    {
        // Generate a spec from the main prompt + clarifications and save the results to the workspace
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsSystemRole($"Instructions: {this.dbs.Input["main_prompt"]}")
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Identity["spec"]);

        this.dbs.Memory["specification"] = messages.Last()["content"];

        return messages;
    }

    public async Task<List<Dictionary<string, string>>> Respec()
    {
        var genSpec = await this.GenSpec();
        var someString = this.dbs.Logs[nameof(genSpec)];
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.dbs.Identity["respec"])
        };
        messages = await this.ai.NextAsync(messages);
        messages = await this.ai.NextAsync(
            messages,
            (
                "Based on the conversation so far, please reiterate the specification for "
                + "the program. If there are things that can be improved, please incorporate the "
                + "improvements. If you are satisfied with the specification, just write out the "
                + "specification word by word again."
            )
        );


        this.dbs.Memory["specification"] = messages.Last()["content"];
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> GenUnitTests()
    {
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input["main_prompt"]}"), this.ai.AsUserRole($"Specification:\n\n{this.dbs.Memory["specification"]}")
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Identity["unit_tests"]);
        this.dbs.Memory["unit_tests"] = messages.Last()["content"];
        this.ToFiles(this.dbs.Memory["unit_tests"], this.dbs.Workspace);
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> GenClarifiedCode()
    {
        var messages = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            this.dbs.Logs[nameof(this.Clarify)]);

        messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
        }.Concat(messages.Skip(1)).ToList();

        messages = await this.ai.NextAsync(messages, this.dbs.Identity["use_qa"]);

        this.ToFiles(messages.Last()["content"], this.dbs.Workspace);
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> GenCode()
    {
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsUserRole($"Instructions: {this.dbs.Input["main_prompt"]}"),
            this.ai.AsUserRole($"Specification:\n\n{this.dbs.Memory["specification"]}"), 
            this.ai.AsUserRole($"Unit tests:\n\n{this.dbs.Memory["unit_tests"]}")
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Identity["use_qa"]);
        this.ToFiles(messages.Last()["content"], this.dbs.Workspace);
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> ExecuteUnitTests()
    {
        var command = this.dbs.Workspace["run.bat"];

        Console.WriteLine("Do you want to execute this code?");
        Console.WriteLine();
        Console.WriteLine(command);
        Console.WriteLine();
        Console.WriteLine("If yes, press enter. Otherwise, type \"no\"");
        Console.WriteLine();

        if (!string.IsNullOrEmpty(Console.ReadLine()))
        {
            Console.WriteLine("Ok, not executing the code.");
            return new List<Dictionary<string, string>>();
        }

        Console.WriteLine("Executing the code...");
        Console.WriteLine();

        // TODO should be configurable
        await Process.Start(new ProcessStartInfo
        {
            FileName = "C:\\tools\\Cmder\\cmder.exe",
            Arguments = "run.bat",
            WorkingDirectory = this.dbs.Workspace["path"]
        })?.WaitForExitAsync()!;

        return new List<Dictionary<string, string>>();
    }

    public async Task<List<Dictionary<string, string>>> ExecuteEntrypoint()
    {
        var command = this.dbs.Workspace["run.bat"];

        Console.WriteLine("Do you want to execute this code?");
        Console.WriteLine();
        Console.WriteLine(command);
        Console.WriteLine();
        Console.WriteLine("If yes, press enter. Otherwise, type \"no\"");
        Console.WriteLine();

        if (!string.IsNullOrEmpty(Console.ReadLine()))
        {
            Console.WriteLine("Ok, not executing the code.");
            return new List<Dictionary<string, string>>();
        }

        Console.WriteLine("Executing the code...");
        Console.WriteLine();

        // TODO should be configurable
        await Process.Start(new ProcessStartInfo
        {
            FileName = "C:\\tools\\Cmder\\cmder.exe",
            Arguments = "run.bat",
            WorkingDirectory = this.dbs.Workspace["path"]
        })?.WaitForExitAsync()!;

        return new List<Dictionary<string, string>>();
    }

    public async Task<List<Dictionary<string, string>>> GenEntrypoint()
    {
        List<Dictionary<string, string>> messages = await this.ai.Start(
            system: "You will get information about a codebase that is currently on disk in the current folder.\n" +
            "From this you will answer with code blocks that include all the necessary " +
            "Unix terminal commands to:\n" +
            "a) Install dependencies\n" +
            "b) Run all necessary parts of the codebase (in parallel if necessary).\n" +
            "Do not install globally. Do not use sudo.\n" +
            "Do not explain the code, just give the commands.",
            user: "Information about the codebase:\n\n" + this.dbs.Workspace["all_output.txt"]
        );
        Console.WriteLine();
        
        var regex = new Regex(@"```\S*\n(.+?)```", RegexOptions.Singleline); // huh?
        var matches = regex.Matches(messages.Last()["content"]);
        this.dbs.Workspace["run.bat"] = string.Join("\n", matches.Select(match => match.Groups[1].Value));
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> UseFeedback()
    {
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input["main_prompt"]}"), this.ai.AsAssistantRole(this.dbs.Workspace["all_output.txt"]), this.ai.AsSystemRole(this.dbs.Identity["use_feedback"])
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Memory["feedback"]);
        this.ToFiles(messages.Last()["content"], this.dbs.Workspace);
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> FixCode()
    {
        var codeOutput = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(this.dbs.Logs[nameof(this.GenCode)]).Last()["content"];
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input["main_prompt"]}"), this.ai.AsUserRole(codeOutput), this.ai.AsSystemRole(this.dbs.Identity["fix_code"])
        };

        messages = await this.ai.NextAsync(messages, "Please fix any errors in the code above.");
        this.ToFiles(messages.Last()["content"], this.dbs.Workspace);
        return messages;
    }
    
    private void ToFiles(string s, DataStore workspace)
    {
        workspace["all_output.txt"] = s;
        var files = this.ParseChat(s);
        foreach (var fileName in files)
        {
            // ??
            workspace[fileName.Item1] = fileName.Item2;
        }
    }
    public List<Tuple<string, string>> ParseChat(string chat)
    {
        // Get all ``` blocks and preceding filenames
        var matches = regex.Matches(chat);

        var files = new List<Tuple<string, string>>();
        foreach (Match match in matches)
        {
            // Strip the filename of any non-allowed characters and convert / to \
            string path = Regex.Replace(match.Groups[1].Value, @"[<>""|?*]", "");

            // Remove leading and trailing brackets
            path = Regex.Replace(path, @"^\[(.*)\]$", "$1");

            // Get the code
            string code = match.Groups[2].Value;

            // Add the file to the list
            files.Add(new Tuple<string, string>(path, code));
        }

        // Get all the text before the first ``` block
        string readme = chat.Split("```")[0];
        files.Add(new Tuple<string, string>("README.md", readme));

        // Return the files
        return files;
    }
}