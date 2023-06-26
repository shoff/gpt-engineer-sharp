namespace GptEngineer.Infrastructure.Services;

public interface IFileSystem
{
    string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
}