namespace GptEngineer.Infrastructure.Extensions;

using Core;
using Core.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;

public static class ConfigurationExtensions
{

    // TODO Extensions for ALL configurable items

    public static CertificateOptions? CertificateOptions(this IConfiguration configuration)
    {
        return configuration.GetNullableOptions<CertificateOptions>(CERTIFICATE_OPTIONS);
    }

    public static HostOptions? HostOptions(this IConfiguration configuration)
    {
        return configuration.GetNullableOptions<HostOptions>(HOST_OPTIONS);
    }
    // Testable until the section.Get<T>
    // ReSharper disable once MemberCanBePrivate.Global
    public static T GetOptions<T>(this IConfiguration configuration,
        string sectionName) where T : IValidatable?
    {
        var section = configuration.GetSection(sectionName);

        if (section == null)
        {
            Log.Logger.Warning("Could not create {SectionName}", sectionName);
            // it REALLY sucks to get a NullReferenceException at app start!
            throw new UnableToCreateOptionsException($"Could not create {sectionName}");
        }

        var options = section.Get<T>();

        if (options == null)
        {
            var message = $"Could not create {sectionName}";
            Log.Logger.Warning("{Message}", message);

            // it REALLY sucks to get a NullReferenceException at app start!
            throw new UnableToCreateOptionsException(message);
        }
        options.Validate();
        return options;
    }

    private static T? GetNullableOptions<T>(this IConfiguration configuration,
        string sectionName)
        where T : IValidatable?
    {
        var section = configuration.GetSection(sectionName);

        if (section == null!)
        {
            Log.Logger.Warning("Could not create {SectionName} 'configuration.GetSection' returned null.", sectionName);
            return default;
        }

        var options = section.Get<T>();

        if (options == null)
        {
            Log.Logger.Warning("Could not create {SectionName} 'section.Get<{Name}>()' returned null.", sectionName, typeof(T).Name);
            return default;
        }

        options.Validate();

        return options;
    }
}

[Serializable]
public class UnableToCreateOptionsException : Exception
{
    public UnableToCreateOptionsException(string message)
        : base(message)
    {
    }
}