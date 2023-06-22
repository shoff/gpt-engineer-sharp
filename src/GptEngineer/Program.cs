namespace GptEngineer;

using System.Text.Json;
using OpenAI;
using OpenAI.Managers;

class Program
{
    public static OpenAIService AIService { get; private set; } = new OpenAIService(new OpenAiOptions()
    {
        ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    });

    public static async Task Main(string[] args)
    {
        var options = new AIOptions();
        options.ProjectPath = args[0] ?? "example"; // "example",

        GetProject();

        options.Model = args[1] ?? "gpt-4";

        options.DeleteExisting = bool.Parse(args[2] ?? "false"); // false,
        options.Temperature = 0.1;
        options.StepsConfig = "default";
        options.Verbose = false;
        options.RunPrefix = "";

        string inputPath = Path.GetFullPath(options.ProjectPath);
        string memoryPath = Path.Combine(inputPath, options.RunPrefix + "memory");
        string workspacePath = Path.Combine(inputPath, options.RunPrefix + "workspace");

        AIService.SetDefaultModelId(options.Model);

        if (options.DeleteExisting)
        {
            // Delete files and subdirectories in paths
            Directory.Delete(memoryPath, true);
            Directory.Delete(workspacePath, true);
        }

        AI ai = new AI(options);

        DBs dbs = new DBs
        {
            Memory = new DB(memoryPath),
            Logs = new DB(Path.Combine(memoryPath, "logs")),
            Input = new DB(inputPath),
            Workspace = new DB(workspacePath),
            Identity = new DB(Path.Combine(Directory.GetCurrentDirectory(), "identity"))
        };
        var stepRunner = new StepRunner(ai, dbs);

        foreach (var step in stepRunner.Default)
        {
            var messages = await step();
            dbs.Logs[step.GetType().Name] = JsonSerializer.Serialize(messages);
        }

        void GetProject()
        {
            bool isCorrect = false;
            while (!isCorrect)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"setting ProjectPath: {options.ProjectPath}");
                Console.WriteLine($"is this correct? (y/n)");
                var response = Console.ReadLine()!.Trim();
                Console.ResetColor();

                if (response != "y")
                {
                    Console.WriteLine("Please enter correct project folder full path:");
                    options.ProjectPath = Console.ReadLine()!.Trim();
                }
                else
                {
                    isCorrect = true;
                }
            }
        }
    }
}

public class StepRunner
{
    private readonly Steps steps;
    public StepRunner(AI ai, DBs dbs)
    {
        this.steps = new Steps(ai, dbs);
    }

    public List<Func<Task<List<Dictionary<string, string>>>>> Default =>
        new()
        {
            this.steps.GenSpec,
            this.steps.GenUnitTests,
            this.steps.GenCode,
            this.steps.GenEntrypoint,
            this.steps.ExecuteEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> Benchmark =>
        new()
        {
            this.steps.GenSpec,
            this.steps.GenUnitTests,
            this.steps.GenCode,
            this.steps.FixCode,
            this.steps.GenEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> Simple =>
        new()
        {
            this.steps.SimpleGen,
            this.steps.GenEntrypoint,
            this.steps.ExecuteEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> Clarify =>
        new()
        {
            this.steps.Clarify,
            this.steps.GenClarifiedCode,
            this.steps.GenEntrypoint,
            this.steps.ExecuteEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> Respec =>
        new()
        {
            this.steps.GenSpec,
            this.steps.Respec,
            this.steps.GenUnitTests,
            this.steps.GenCode,
            this.steps.GenEntrypoint,
            this.steps.ExecuteEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> ExecuteOnly =>
        new()
        {
            this.steps.ExecuteEntrypoint
        };

    public List<Func<Task<List<Dictionary<string, string>>>>> UseFeedback =>
        new()
        {
            this.steps.UseFeedback
        };
}