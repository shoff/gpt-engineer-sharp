namespace GptEngineer.Core.Stores;

public interface IStepStore
{
    string this[string key, string? role = null] { get; set; }
}