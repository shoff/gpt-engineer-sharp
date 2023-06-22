namespace GptEngineer;

public class DBs : IDBs
{
    public DB Memory { get; set; } = new("memory");
    public DB Logs { get; set; } = new("logs");
    public DB Identity { get; set; } = new("identity");
    public DB Input { get; set; } = new("input");
    public DB Workspace { get; set; } = new("workspace");

    public DB this[string input]
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