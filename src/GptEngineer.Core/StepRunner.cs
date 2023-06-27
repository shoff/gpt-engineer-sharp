using GptEngineer.Core.Stores;

namespace GptEngineer.Core;

using StepDefinitions;

public class StepRunner : IStepRunner
{
    private readonly IClarify clarify;
    private readonly IGenerateCode generateCode;
    private readonly IGenerateEntrypoint generateEntrypoint;
    private readonly IGenerateUnitTests generateUnitTests;
    private readonly IExecuteEntrypoint executeEntrypoint;
    private readonly IGenerateSpecification generateSpecification;
    private readonly Steps steps;

    public StepRunner(
        IAI ai, 
        IDataStores dbs,
        IWorkspaceStore workspaceStore,
        IClarify clarify,
        IGenerateCode generateCode,
        IGenerateEntrypoint generateEntrypoint,
        IGenerateUnitTests generateUnitTests,
        IExecuteEntrypoint executeEntrypoint,
        IGenerateSpecification generateSpecification)
    {
        this.clarify = clarify;
        this.generateCode = generateCode;
        this.generateEntrypoint = generateEntrypoint;
        this.generateUnitTests = generateUnitTests;
        this.executeEntrypoint = executeEntrypoint;
        this.generateSpecification = generateSpecification;
        this.steps = new Steps(ai, dbs, workspaceStore);
    }

    // Should be able to now run default steps hahahahahah yeah right
    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Default
    {
        get
        {
            yield return this.generateSpecification.RunAsync;
            yield return this.generateUnitTests.RunAsync;
            yield return this.generateCode.RunAsync;
            yield return this.generateEntrypoint.RunAsync;
            yield return this.executeEntrypoint.RunAsync;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Benchmark
    {
        get
        {
            yield return this.generateSpecification.RunAsync;
            yield return this.generateUnitTests.RunAsync;
            yield return this.generateCode.RunAsync;
            yield return this.steps.FixCode;
            yield return this.generateEntrypoint.RunAsync;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Simple
    {
        get
        {
            yield return this.steps.SimpleGen;
            yield return this.generateEntrypoint.RunAsync;
            yield return this.executeEntrypoint.RunAsync;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Clarify
    {
        get
        {
            yield return this.clarify.RunAsync;
            yield return this.steps.GenClarifiedCode;
            yield return this.generateEntrypoint.RunAsync;
            yield return this.executeEntrypoint.RunAsync;
        }
    }

    public IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Respec
    {
        get
        {
            yield return this.generateSpecification.RunAsync;
            yield return this.steps.Respec;
            yield return this.generateUnitTests.RunAsync;
            yield return this.generateCode.RunAsync;
            yield return this.generateEntrypoint.RunAsync;
            yield return this.executeEntrypoint.RunAsync;
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