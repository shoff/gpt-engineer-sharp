namespace GptEngineer.Core.Projects;

using System.IO;

// just basing this off of the folder structure the original author used
public class Project
{
    private const string MEMORY = "/memory";
    private const string  WORKSPACE = "/workspace";

    public Project(string projectDirectory)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectDirectory, nameof(projectDirectory));
        this.Path = projectDirectory;
        CreateIfNotExists(this.Path);
        this.Name = System.IO.Path.GetDirectoryName(projectDirectory) ?? "Could not read directory name!";
        this.Memory = new Memory($"{this.Path}{MEMORY}");
        this.Workspace = new Workspace($"{this.Path}{WORKSPACE}");
    }

    public bool HasWorkspace => Directory.Exists(this.Path + WORKSPACE);
    public bool HasMemory => Directory.Exists(this.Path + MEMORY);
    public string Name { get; init; }
    public string? Path { get; init; }
    public string Description { get; set; } = string.Empty;
    public Workspace Workspace { get; }
    public Memory Memory { get; }
    public string MemoryPath => this.Path + MEMORY;
    public string WorkspacePath => this.Path + WORKSPACE;
    public ICollection<string> Errors => new HashSet<string>();
    public async Task LoadAsync()
    {
        if (this.HasWorkspace)
        {
            await this.Workspace.FillAsync(this.Path + WORKSPACE);
        }

        if (this.Memory.HasLogs)
        {
            await this.Memory.GptLog.FillAsync(this.Memory.Path + "/logs");
        }

        await this.Memory.FillAsync(this.Memory.Path + "/specification");
        await this.Memory.FillAsync(this.Memory.Path + "/unit_test");
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