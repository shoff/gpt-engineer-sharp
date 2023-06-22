namespace GptEngineer;

public interface IStepRunner
{
    List<Func<Task<List<Dictionary<string, string>>>>> Default { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> Benchmark { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> Simple { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> Clarify { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> Respec { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> ExecuteOnly { get; }
    List<Func<Task<List<Dictionary<string, string>>>>> UseFeedback { get; }
}