using GptEngineer.Core.Stores;

namespace GptEngineer.Core;

public class StepRunner : IStepRunner
{
    private readonly Steps steps;
    public StepRunner(
        IAI ai, 
        IDataStores dbs,
        IWorkspaceStore workspaceStore)
    {
        this.steps = new Steps(ai, dbs, workspaceStore);
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Default
    {
        get
        {
            yield return this.steps.GenSpec;
            yield return this.steps.GenUnitTests;
            yield return this.steps.GenCode;
            yield return this.steps.GenEntrypoint;
            yield return this.steps.ExecuteEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Benchmark
    {
        get
        {
            yield return this.steps.GenSpec;
            yield return this.steps.GenUnitTests;
            yield return this.steps.GenCode;
            yield return this.steps.FixCode;
            yield return this.steps.GenEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Simple
    {
        get
        {
            yield return this.steps.SimpleGen;
            yield return this.steps.GenEntrypoint;
            yield return this.steps.ExecuteEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Clarify
    {
        get
        {

            yield return this.steps.Clarify;
            yield return this.steps.GenClarifiedCode;
            yield return this.steps.GenEntrypoint;
            yield return this.steps.ExecuteEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Respec
    {
        get
        {
            yield return this.steps.GenSpec;
            yield return this.steps.Respec;
            yield return this.steps.GenUnitTests;
            yield return this.steps.GenCode;
            yield return this.steps.GenEntrypoint;
            yield return this.steps.ExecuteEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> ExecuteOnly
    {
        get
        {
            yield return this.steps.ExecuteEntrypoint;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> UseFeedback
    {
        get
        {
            yield return this.steps.UseFeedback;
        }
    }
}