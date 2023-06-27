namespace GptEngineer.Core.Stores;

public interface IWorkspaceStore : IDataStore
{
    void ToFiles(string text);
}