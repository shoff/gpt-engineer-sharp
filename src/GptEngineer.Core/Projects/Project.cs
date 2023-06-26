namespace GptEngineer.Core.Projects;

using System.IO;

// just basing this off of the folder structure the original author used
public class Project
{
    private const string MEMORY = "memory";
    private const string WORKSPACE = "workspace";
    private const string SPECIFICATION = "specification";
    private const string UNIT_TEST = "unit_test";
    private const string LOGS = "logs";
    // ReSharper disable once InconsistentNaming
    private static readonly char SLASH = System.IO.Path.DirectorySeparatorChar;
    
    public Project()
    {
        this.Path = string.Empty;
        this.Name = string.Empty;
        this.Memory = new Memory($"{this.Path}{SLASH}{MEMORY}");
        this.Workspace = new Workspace($"{this.Path}{SLASH}{WORKSPACE}");
    }

    public Project(string projectDirectory)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectDirectory, nameof(projectDirectory));
        this.Path = projectDirectory;
        CreateIfNotExists(this.Path);

        this.Name = System.IO.Path.GetDirectoryName(projectDirectory) ?? "Could not read directory name!";
        this.Memory = new Memory($"{this.Path}{SLASH}{MEMORY}");
        this.Workspace = new Workspace($"{this.Path}{SLASH}{WORKSPACE}");
    }

    public bool HasWorkspace => Directory.Exists($"{this.Path}{SLASH}{WORKSPACE}");
    public bool HasMemory => Directory.Exists(this.Path + MEMORY);
    public string Name { get; set; }
    public string? Path { get; set; }
    public string Description { get; set; } = string.Empty;
    public Workspace Workspace { get; set; }
    public Memory Memory { get; set; }
    public string MemoryPath => $"{this.Path}{SLASH}{MEMORY}";
    public string WorkspacePath => $"{this.Path}{SLASH}{WORKSPACE}";
    public ICollection<string> Errors { get; set; } = new HashSet<string>();
    public async Task LoadAsync()
    {
        if (this.HasWorkspace)
        {
            await this.Workspace.FillAsync($"{this.Path}{SLASH}{WORKSPACE}");
        }

        if (this.Memory.HasLogs)
        {
            await this.Memory.GptLogs
                .FillAsync($"{this.Memory.Path}{SLASH}{LOGS}")
                .ConfigureAwait(false);
        }

        await this.Memory.FillAsync($"{this.Memory.Path}{SLASH}{SPECIFICATION}");
        await this.Memory.FillAsync($"{this.Memory.Path}{SLASH}{UNIT_TEST}");
    }
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