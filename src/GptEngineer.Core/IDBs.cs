namespace GptEngineer;

public interface IDBs
{
    DB Memory { get; set; }
    DB Logs { get; set; }
    DB Identity { get; set; }
    DB Input { get; set; }
    DB Workspace { get; set; }
    DB this[string input] { get; }
}