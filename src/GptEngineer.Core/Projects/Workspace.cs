namespace GptEngineer.Core.Projects;

public class Workspace
{
    private const string WORKSPACE = "/workspace";

    public Workspace()
    {
        this.Path = WORKSPACE;
    }

    public Workspace(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        this.Path = path;
        CreateIfNotExists(path);
    }

    public string Path { get; set; }

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
    public ICollection<string> FileList { get; set; } = new HashSet<string>();
    public ICollection<string> Errors { get; set; }= new HashSet<string>();
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