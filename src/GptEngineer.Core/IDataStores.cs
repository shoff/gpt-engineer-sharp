namespace GptEngineer.Core;

public interface IDataStores
{
    DataStore Memory { get; set; }
    DataStore Logs { get; set; }
    DataStore Identity { get; set; }
    DataStore Input { get; set; }
    DataStore Workspace { get; set; }
    DataStore this[string input] { get; }
}