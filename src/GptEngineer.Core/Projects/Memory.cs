namespace GptEngineer.Core.Projects;

using System.Text.Json;

public class Memory
{
    private const string SPECIFICATION = "specification";
    private const string UNIT_TEST = "unit_test";
    private const string LOGS = "/logs";
    private const string MEMORY = "/memory";

    public Memory()
    {
        this.Path = MEMORY;
        this.GptLogs = new GptLogs(System.IO.Path.Combine(this.Path, LOGS));
    }

    public Memory(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        this.Path = path;
        CreateIfNotExists(path);
        this.GptLogs = new GptLogs(System.IO.Path.Combine(path,  LOGS));
    }

    public async Task FillAsync(string memoryFile)
    {
        if (string.IsNullOrWhiteSpace(memoryFile) || !File.Exists(memoryFile))
        {
            return;
        }

        if (memoryFile == SPECIFICATION)
        {
            var json = await File.ReadAllTextAsync(memoryFile);
            var specification = JsonSerializer.Deserialize<ICollection<GptMessage>>(json);
            this.Specifications = specification;
        }

        if (memoryFile == UNIT_TEST)
        {
            var json = await File.ReadAllTextAsync(memoryFile);
            var unitTests = JsonSerializer.Deserialize<ICollection<GptMessage>>(json);
            this.UnitTests = unitTests;
        }

        if (this.HasLogs)
        {
            await this.GptLogs.FillAsync($"{this.Path}{LOGS}");
        }
        else
        {
            try
            {
                // create it
                Directory.CreateDirectory($"{this.Path}{LOGS}");
            }
            catch (Exception ex)
            {
                this.Errors.Add(ex.Message);
            }
        }
    }

    public bool HasLogs => Directory.Exists($"{this.Path}{LOGS}");
    public GptLogs GptLogs { get; set; }
    public ICollection<GptMessage>? Specifications { get; set; } = new HashSet<GptMessage>();
    public ICollection<GptMessage>? UnitTests { get; set; } = new HashSet<GptMessage>();
    public ICollection<string> Errors => new HashSet<string>();
    public string Path { get; set; }
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