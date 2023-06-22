namespace GptEngineer.Core.Configuration;

public class HostOptions : IValidatable
{
    public int HttpPort { get; set; } = 5000;
    public int HttpsPort { get; set; } = 5001;
    public void Validate()
    {
    }
}