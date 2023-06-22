public class AIOptions
{
    public string? ProjectPath { get; set; } 
    public string Model { get; set; } = "gpt-4";
    public bool DeleteExisting { get; set; }
    public double Temperature { get; set; } = 0.1;
    public string StepsConfig { get; set; } = "default";
    public bool Verbose { get; set; }
    public string RunPrefix { get; set; } = "";
}