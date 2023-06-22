namespace GptEngineer.Core.Configuration;

public sealed class CertificateOptions : IValidatable
{
    public bool UseWindowsStore { get; set; }
    public string? CertificateFileLocation { get; set; }
    public string? CertificateFileName { get; set; }
    public string? CertificatePassword { get; set; }
    public string? CertificateHost { get; set; }
    public string? CertificateStoreLocation { get; set; }
    public string? Issuer { get; set; }
    public string? SerialNumber { get; set; }
    public string? Subject { get; set; }
    public string? Thumbprint { get; set; }
    public int? Version { get; set; }
    public int? CertificateStoreName { get; set; }

    public void Validate()
    {
        // TODO
    }
}