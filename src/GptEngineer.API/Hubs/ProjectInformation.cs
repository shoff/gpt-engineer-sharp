namespace GptEngineer.API.Hubs;

public record ProjectInformation(string ProjectName, int WorkspaceFileCount)
{
    public override string ToString()
    {
        return $"{this.ProjectName} with {this.WorkspaceFileCount} workspace files";
    }
}