

public class FileSystem : IFileSystem
{
    public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.GetDirectories(path, searchPattern, enumerationOptions);
    }
}