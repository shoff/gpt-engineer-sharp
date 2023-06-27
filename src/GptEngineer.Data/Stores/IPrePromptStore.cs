namespace GptEngineer.Data.Stores;

public interface IPrePromptStore
{
    string this[string key] { get; set; }
}