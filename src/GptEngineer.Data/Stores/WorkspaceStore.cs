namespace GptEngineer.Data.Stores;

using System.Text.RegularExpressions;
using Core.Configuration;
using Core.Stores;
using Microsoft.Extensions.Options;

public class WorkspaceStore : DataStore, IWorkspaceStore
{
    private static readonly Regex regex = new(@"(\S+?)\n```\S+\n(.+?)```", RegexOptions.Compiled);
    
    public WorkspaceStore(
        
        // TODO this is a bug because we don't know the project name at this point
        IOptions<WorkspaceOptions> options)
        : base(options.Value.Path)
    {
    }

    public void ToFiles(string text)
    {
        this["all_output.txt"] = text;

        var files = this.ParseChat(text);

        foreach (var fileName in files)
        {
            // ??
            this[fileName.Item1] = fileName.Item2;
        }
    }


    private IEnumerable<Tuple<string, string>> ParseChat(string chat)
    {
        // Get all ``` blocks and preceding file names
        var matches = regex.Matches(chat);

        var files = new List<Tuple<string, string>>();
        foreach (Match match in matches)
        {
            // Strip the filename of any non-allowed characters and convert / to \
            string path = Regex.Replace(match.Groups[1].Value, @"[<>""|?*]", "");

            // Remove leading and trailing brackets
            path = Regex.Replace(path, @"^\[(.*)\]$", "$1");

            // Get the code
            string code = match.Groups[2].Value;

            // Add the file to the IEnumerable
            files.Add(new Tuple<string, string>(path, code));
        }

        // Get all the text before the first ``` block
        string readme = chat.Split("```")[0];
        files.Add(new Tuple<string, string>("README.md", readme));

        // Return the files
        return files;
    }


}