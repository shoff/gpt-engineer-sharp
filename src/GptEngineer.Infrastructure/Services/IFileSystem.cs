public interface IFileSystem
{
    string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
}