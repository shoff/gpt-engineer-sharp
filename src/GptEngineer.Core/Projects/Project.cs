namespace GptEngineer.Core.Projects;

using System.IO;

// just basing this off of the folder structure the original author used
public class Project
{
    private const string MEMORY = "/memory";
    private const string WORKSPACE = "/workspace";

    public Project(string projectDirectory)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectDirectory, nameof(projectDirectory));
        Path = projectDirectory;
        Name = System.IO.Path.GetDirectoryName(projectDirectory) ?? "Could not read directory name!";
        Memory = new(Path + MEMORY);
    }

    public bool HasWorkspace => Directory.Exists(Path + WORKSPACE);
    public bool HasMemory => Directory.Exists(Path + MEMORY);
    public string Name { get; init; }
    public string? Path { get; init; }
    public string Description { get; set; } = string.Empty;

    public Workspace Workspace { get; } = new();
    public Memory Memory { get; }

    public string MemoryPath => Path + MEMORY;
    public string WorkspacePath => Path + WORKSPACE;

    public async Task LoadAsync()
    {
        if (HasWorkspace)
        {
            await Workspace.FillAsync(Path + WORKSPACE);
        }

        if (Memory.HasLogs)
        {
            await Memory.Log.FillAsync(Memory.Path + "/logs");
        }

        await Memory.FillAsync(Memory.Path + "/specification");
        await Memory.FillAsync(Memory.Path + "/unit_test");
    }
}