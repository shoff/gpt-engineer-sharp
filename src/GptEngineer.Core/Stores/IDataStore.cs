namespace GptEngineer.Core.Stores;

public interface IDataStore
{
    string this[string key] { get; set; }
}