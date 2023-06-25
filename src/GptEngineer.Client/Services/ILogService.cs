namespace GptEngineer.Client.Services;

public interface ILogService
{
    Task LogError(string message);
}