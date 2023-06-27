// ReSharper disable ParameterHidesMember
namespace GptEngineer.Core;

using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using GptEngineer.Core.Stores;

public class Steps : ISteps
{
    private const string GEN_SPEC_MESSAGE = "Based on the conversation so far, please reiterate the specification for the program. If there are things that can be improved, please incorporate the improvements. If you are satisfied with the specification, just write out the specification word by word again.";

    private readonly IDataStores dbs;
    private readonly IWorkspaceStore workspaceStore;
    private readonly IAI ai;
    private static readonly Regex regex = new(@"(\S+?)\n```\S+\n(.+?)```", RegexOptions.Compiled);

    public Steps(IAI ai, IDataStores dbs, 
        IWorkspaceStore workspaceStore)
    {
        this.ai = ai;
        this.dbs = dbs;
        this.workspaceStore = workspaceStore;
    }

    public string SetupSysPrompt()
    {
        return this.dbs.Identity["generate"] + "\nUseful to know:\n" + this.dbs.Identity["philosophy"];
    }

    public async Task<IEnumerable<Dictionary<string, string>>> SimpleGen()
    {
        // Run the AI on the main prompt and save the results
        IEnumerable<Dictionary<string, string>> messages = await this.ai.Start(this.SetupSysPrompt(), this.dbs.Input[MAIN_PROMPT]);

        var simpleGen = messages as Dictionary<string, string>[] ?? messages.ToArray();
        
        if (simpleGen.Length > 0)
        {
            this.ToFiles(simpleGen.Last()[CONTENT], this.workspaceStore);
            return simpleGen;
        }

        return simpleGen;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> Clarify()
    {
        // TODO This is clearly incorrect 
        var messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.dbs.Identity["qa"])
        };
        var user = this.dbs.Input[MAIN_PROMPT]; // user is the main prompt

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

    public async Task<IEnumerable<Dictionary<string, string>>> GenSpec()
    {
        // Generate a spec from the main prompt + clarifications and save the
        // results to the workspace
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsSystemRole($"Instructions: {this.dbs.Input[MAIN_PROMPT]}")
        };

        // the call to next must persist or something
        messages = await this.ai.NextAsync(messages, this.dbs.Identity[SPEC]);
        var genSpec = messages as Dictionary<string, string>[] ?? messages.ToArray();
        
        
        this.dbs.Memory[SPECIFICATION] = genSpec.Last()[CONTENT];
        return genSpec;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> Respec()
    {
        // ReSharper disable once RedundantAssignment
        var genSpec = await this.GenSpec();

        var someString = this.dbs.Logs[nameof(genSpec)];

        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.dbs.Identity[RESPEC])
        };

        messages = await this.ai.NextAsync(messages);

        messages = await this.ai.NextAsync(
            messages,
            (
                GEN_SPEC_MESSAGE
            )
        );


        this.dbs.Memory[SPECIFICATION] = messages.Last()[CONTENT];
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GenUnitTests()
    {
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input[MAIN_PROMPT]}"), this.ai.AsUserRole($"Specification:\n\n{this.dbs.Memory[SPECIFICATION]}")
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Identity[UNIT_TESTS]);
        this.dbs.Memory[UNIT_TESTS] = messages.Last()[CONTENT];
        this.ToFiles(this.dbs.Memory[UNIT_TESTS], this.workspaceStore);
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GenClarifiedCode()
    {
        // TODO this code makes no sense at all
        var messageList =
            JsonSerializer.Deserialize<List<Dictionary<string, string>>>(this.dbs.Logs[nameof(this.Clarify)]);

        IEnumerable<Dictionary<string, string>> messages = messageList;
        messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
        }.Concat(messages.Skip(1));

        messages = await this.ai.NextAsync(messages, this.dbs.Identity[USE_QA]);

        this.ToFiles(messages.Last()[CONTENT], this.workspaceStore);
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GenCode()
    {
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()),
            this.ai.AsUserRole($"Instructions: {this.dbs.Input[MAIN_PROMPT]}"),
            this.ai.AsUserRole($"Specification:\n\n{this.dbs.Memory[SPECIFICATION]}"),
            this.ai.AsUserRole($"Unit tests:\n\n{this.dbs.Memory[UNIT_TESTS]}")
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Identity[USE_QA]);
        this.ToFiles(messages.Last()[CONTENT], this.workspaceStore);
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> ExecuteUnitTests()
    {
        var command = this.dbs.Workspace["run.bat"];

        Console.WriteLine("Do you want to execute this code?");
        Console.WriteLine();
        Console.WriteLine(command);
        Console.WriteLine();
        Console.WriteLine("If yes, press enter. Otherwise, type \"no\"");
        Console.WriteLine();

        // TODO NFI how to implement this the way the code is written
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
            WorkingDirectory = this.dbs.Workspace["path"]
        })?.WaitForExitAsync()!;

        return new List<Dictionary<string, string>>();
    }

    public async Task<IEnumerable<Dictionary<string, string>>> ExecuteEntrypoint()
    {
        var command = this.dbs.Workspace["run.bat"];

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
            WorkingDirectory = this.dbs.Workspace["path"]
        })?.WaitForExitAsync()!;

        return new List<Dictionary<string, string>>();
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GenEntrypoint()
    {
        IEnumerable<Dictionary<string, string>> messages = await this.ai.Start(
            system: "You will get information about a codebase that is currently on disk in the current folder.\n" +
            "From this you will answer with code blocks that include all the necessary " +
            "Unix terminal commands to:\n" +
            "a) Install dependencies\n" +
            "b) Run all necessary parts of the codebase (in parallel if necessary).\n" +
            "Do not install globally. Do not use sudo.\n" +
            "Do not explain the code, just give the commands.",
            user: "Information about the codebase:\n\n" + this.dbs.Workspace["all_output.txt"]
        );
        // Console.WriteLine();

        var regex = new Regex(@"```\S*\n(.+?)```", RegexOptions.Singleline); // huh?
        var matches = regex.Matches(messages.Last()[CONTENT]);
        this.dbs.Workspace["run.bat"] = string.Join("\n", matches.Select(match => match.Groups[1].Value));
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> UseFeedback()
    {
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input[MAIN_PROMPT]}"), this.ai.AsAssistantRole(this.dbs.Workspace["all_output.txt"]), this.ai.AsSystemRole(this.dbs.Identity["use_feedback"])
        };

        messages = await this.ai.NextAsync(messages, this.dbs.Memory["feedback"]);
        this.ToFiles(messages.Last()[CONTENT], this.workspaceStore);
        return messages;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> FixCode()
    {
        var codeOutput = JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(this.dbs.Logs[nameof(this.GenCode)]).Last()[CONTENT];
        IEnumerable<Dictionary<string, string>> messages = new List<Dictionary<string, string>>
        {
            this.ai.AsSystemRole(this.SetupSysPrompt()), this.ai.AsUserRole($"Instructions: {this.dbs.Input[MAIN_PROMPT]}"), this.ai.AsUserRole(codeOutput), this.ai.AsSystemRole(this.dbs.Identity["fix_code"])
        };

        messages = await this.ai.NextAsync(messages, "Please fix any errors in the code above.");
        this.ToFiles(messages.Last()[CONTENT], this.workspaceStore);
        return messages;
    }
    
    private void ToFiles(string text, IWorkspaceStore workspaceStore)
    {
        workspaceStore["all_output.txt"] = text;

        var files = this.ParseChat(text);
        
        foreach (var fileName in files)
        {
            // ??
            workspaceStore[fileName.Item1] = fileName.Item2;
        }
    }
    
    public IEnumerable<Tuple<string, string>> ParseChat(string chat)
    {
        // Get all ``` blocks and preceding file names
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

            // Add the file to the IEnumerable
            files.Add(new Tuple<string, string>(path, code));
        }

        // Get all the text before the first ``` block
        string readme = chat.Split("```")[0];
        files.Add(new Tuple<string, string>("README.md", readme));

        // Return the files
        return files;
    }
}