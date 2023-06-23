namespace GptEngineer.Core.Projects;

public class Workspace
{
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

}