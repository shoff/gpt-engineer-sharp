namespace GptEngineer;

public class StepRunner : IStepRunner
{
    private readonly Steps steps;
    public StepRunner(IAI ai, IDBs dbs)
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