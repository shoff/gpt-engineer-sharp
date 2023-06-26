using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;

namespace GptEngineer.Core.Configuration;

[ExcludeFromCodeCoverage]
public sealed record RedisOptions : IValidatable
{
    private static readonly Validator validator = new();

    public bool Enabled { get; set; } = true;
    public string Configuration { get; set; } = null!;
    public string InstanceName { get; set; } = null!;
    public RedisConfiguration ConfigurationOptions { get; set; } = new();


    public void Validate()
    {
        validator.ValidateAndThrow(this);
    }

    internal class Validator : AbstractValidator<RedisOptions>
    {
        public Validator()
        {
            this.RuleFor(c => c.Configuration).NotEmpty().WithMessage("Configuration is required");
            this.RuleFor(c => c.InstanceName).NotEmpty().WithMessage("InstanceName is required");
            this.RuleFor(c => c.ConfigurationOptions).NotNull().WithMessage("ConfigurationOptions cannot be null");
        }
    }
}

[ExcludeFromCodeCoverage]
public sealed class RedisConfiguration
{
    public bool AbortOnConnectFail { get; set; }
    public bool AllowAdmin { get; set; }
    public int AsyncTimeout { get; set; } = 5000;
    public int SyncTimeout { get; set; } = 5000;
    public bool UseSsl { get; set; } = true;
    public string ChannelPrefix { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public int ConnectRetry { get; set; } = 3;
    public bool CheckCertificateRevocation { get; set; }
    public string ConfigurationChannel { get; set; } = "__MasterChanged";
    public int ConnectTimeout { get; set; } = 5000;
    public int? DefaultDatabase { get; set; }
    public ICollection<RedisEndpoint> Endpoints { get; set; } = new List<RedisEndpoint>();
    public bool HighPrioritySocketThreads { get; set; }
    public int KeepAlive { get; set; } = 60;
    public string Password { get; set; } = null!;
    public int Proxy { get; set; }
    public string ServiceName { get; set; } = null!;
    public string SslHost { get; set; } = null!;
    public string TieBreaker { get; set; } = null!;
    public SslProtocols SslProtocols { get; set; }
    public int ConfigCheckSeconds { get; set; } = 30;
    public HashSet<string> Commands { get; set; } = new()
    {
        "INFO", "CONFIG", "CLUSTER", "PING", "ECHO", "CLIENT"
    };
}
[ExcludeFromCodeCoverage]

public sealed record RedisEndpoint
{
    public string Host { get; set; } = null!;
    public int AddressFamily { get; set; } = 0;
    public int Port { get; set; } = 6379;
}