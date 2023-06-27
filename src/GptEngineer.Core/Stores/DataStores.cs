namespace GptEngineer.Core;
using GptEngineer.Core.Stores;

public class DataStores : IDataStores
{
    public DataStore Memory { get; set; } = new("memory");
    public DataStore Logs { get; set; } = new("logs");
    public DataStore Identity { get; set; } = new("identity");
    public DataStore Input { get; set; } = new("input");
    public DataStore Workspace { get; set; } = new("workspace");

    public DataStore this[string input]
    {
        get
        {
            return input switch
            {
                "memory" => this.Memory,
                "logs" => this.Logs,
                "identity" => this.Identity,
                "input" => this.Input,
                "workspace" => this.Workspace,
                _ => throw new KeyNotFoundException(input)
            };
        }
    }
}