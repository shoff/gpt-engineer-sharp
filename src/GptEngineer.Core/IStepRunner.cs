namespace GptEngineer.Core;

public interface IStepRunner
{
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Default { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Benchmark { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Simple { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Clarify { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> Respec { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> ExecuteOnly { get; }
    IEnumerable<Func<Task<IEnumerable<Dictionary<string, string>>>>> UseFeedback { get; }
}