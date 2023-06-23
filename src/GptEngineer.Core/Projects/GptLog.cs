namespace GptEngineer.Core.Projects;

using System.Text.Json;

public class GptLog
{
    public async Task FillAsync(string logDirectory)
    {
        var enumerationOptions = new EnumerationOptions()
        {
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            RecurseSubdirectories = false
        };

        var logsDirectory = Directory.GetFiles(logDirectory, "*", enumerationOptions);

        foreach (var logFile in logsDirectory)
        {
            if (logFile == "clarify")
            {
                var json = await File.ReadAllTextAsync(logFile);
                var clarifyingQuestions = JsonSerializer.Deserialize<GptMessage[]>(json);
                foreach (var clarifyingQuestion in clarifyingQuestions ?? Array.Empty<GptMessage>())
                {
                    this.Clarifications.Add(clarifyingQuestion);
                }
            }

            if (logFile == "clarify_ran")
            {
                var json = await File.ReadAllTextAsync(logFile);
                var clarifyingQuestionsRan = JsonSerializer.Deserialize<GptMessage[]>(json);
                foreach (var clarifyingQuestionRan in clarifyingQuestionsRan ?? Array.Empty<GptMessage>())
                {
                    this.ClarificationsRan.Add(clarifyingQuestionRan);
                }
            }
        }
    }
    public ICollection<GptMessage> Clarifications { get; } = new List<GptMessage>();
    public ICollection<GptMessage> ClarificationsRan { get; } = new List<GptMessage>();
}