namespace GptEngineer.Core.Projects;

public class Workspace
{
    public Workspace(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        this.Path = path;
        CreateIfNotExists(path);
    }

    public string Path { get; }

    public Task FillAsync(string folder)
    {
        var enumerationOptions = new EnumerationOptions()
        {
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            RecurseSubdirectories = false
        };
        var files = Directory.GetFiles(folder, "*", enumerationOptions);
        foreach (var file in files)
        {
            this.FileList.Add(file);
        }
        return Task.CompletedTask;
    }
    public ICollection<string> FileList { get; } = new HashSet<string>();
    public ICollection<string> Errors => new HashSet<string>();
    private void CreateIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                this.Errors.Add(e.Message);
            }
        }
    }
}