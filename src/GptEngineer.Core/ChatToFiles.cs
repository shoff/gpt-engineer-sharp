namespace GptEngineer.Core;

using System.Text.RegularExpressions;

public class ChatToFiles
{
    public List<Tuple<string, string>> ParseChat(string chat)
    {
        // Get all ``` blocks and preceding filenames
        string pattern = @"(\S+?)\n```\S+\n(.+?)```";
        Regex regex = new Regex(pattern, RegexOptions.Singleline);

        List<Tuple<string, string>> files = new List<Tuple<string, string>>();
        foreach (Match match in regex.Matches(chat))
        {
            // Strip the filename of any non-allowed characters and convert / to \
            string path = Regex.Replace(match.Groups[1].Value, @"[<>""|?*]", "");

            // Remove leading and trailing brackets
            path = Regex.Replace(path, @"^\[(.*)\]$", "$1");

            // Get the code
            string code = match.Groups[2].Value;

            // Add the file to the list
            files.Add(new Tuple<string, string>(path, code));
        }

        // Get all the text before the first ``` block
        string readme = chat.Split("```")[0];
        files.Add(new Tuple<string, string>("README.md", readme));

        // Return the files
        return files;
    }

    public void ToFiles(string chat, Dictionary<string, string> workspace)
    {
        workspace["all_output.txt"] = chat;

        List<Tuple<string, string>> files = this.ParseChat(chat);
        foreach (var file in files)
        {
            workspace[file.Item1] = file.Item2;
        }
    }
}