namespace GptEngineer.Core.Projects;

using System.Text.Json;

public class Memory
{

    public Memory(string path)
    {
        this.Path = path;
    }

    public async Task FillAsync(string memoryFile)
    {
        if (string.IsNullOrWhiteSpace(memoryFile) || !File.Exists(memoryFile))
        {
            return;
        }

        if (memoryFile == "specification")
        {
            var json = await File.ReadAllTextAsync(memoryFile);
            var specification = JsonSerializer.Deserialize<ICollection<GptMessage>>(json);
            this.Specifications = specification;
        }

        if (memoryFile == "unit_test")
        {
            var json = await File.ReadAllTextAsync(memoryFile);
            var unitTests = JsonSerializer.Deserialize<ICollection<GptMessage>>(json);
            this.UnitTests = unitTests;
        }

        if (this.HasLogs)
        {
            await this.Log.FillAsync(this.Path + "/logs");
        }
    }

    public bool HasLogs => Directory.Exists(this.Path + "/logs");

    public GptLog Log { get; } = new();

    public ICollection<GptMessage>? Specifications { get; private set; } = new HashSet<GptMessage>();

    public ICollection<GptMessage>? UnitTests { get; private set; } = new HashSet<GptMessage>();

    public string Path { get; init; }

}