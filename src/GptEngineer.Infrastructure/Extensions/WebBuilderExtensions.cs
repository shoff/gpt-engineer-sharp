namespace GptEngineer.Infrastructure.Extensions;

using GptEngineer.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.Security.Cryptography.X509Certificates;

public static class WebBuilderExtensions
{
    internal const string X509_CERT = "X509CERT";
    internal const string X509_CERT_PASS = "X509CERT_PASS";

    public static void ConfigureBuilderDefaults(
    this WebApplicationBuilder builder,
    string[]? args)
    {
        builder.Host
            // Sets up the configuration for the remainder of the build process and application.
            // The Configuration passed in is the host's configuration built from calls to
            // ConfigureHostConfiguration(Action<IConfigurationBuilder>).
            // This can be called multiple times and the results will be additive.
            // After all calls have been processed, Configuration will be
            // updated with the results for future build steps.
            // The resulting configuration will also be available in the Services DI Container.
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder.configureappconfiguration?view=dotnet-plat-ext-6.0
#pragma warning disable ASP0013
#pragma warning disable ASP0013
            .ConfigureAppConfiguration((_, _) =>
            {
                //if (hostEnvironment.IsLocal() && Array.IndexOf(args, USE_LOCAL_SECRETS) != -1)
                //{
                //    Log.Logger.Information("adding local user secrets for environment");
                //    builder.Configuration.AddLocalUserSecrets(hostEnvironment.Environment);
                //}
            });
#pragma warning restore ASP0013
#pragma warning restore ASP0013
        // 
        builder.WebHost.UseKestrel((builderContext, options) =>
        {
            options.Limits.Http2.HeaderTableSize = 4096;
            options.Limits.Http2.MaxStreamsPerConnection = 100;
            options.Limits.Http2.MaxFrameSize = 16_384;
            options.Limits.Http2.MaxRequestHeaderFieldSize = 8192;

            var hostOptions = builderContext
                .Configuration.HostOptions();
            var certificateOptions = builderContext
                .Configuration.CertificateOptions();

            X509Certificate2? sslCert = null;

            if (certificateOptions != null)
            {
                sslCert = certificateOptions.LoadCertificate();
            }

            sslCert ??= LoadFromEnvironmentVariable();

            // NOTE If using a reverse proxy to kestrel, then 
            // the ssl cert is most likely not needed as the TLS stream
            // is terminated at the proxy.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (sslCert is not null)
            {
                var httpsPort = hostOptions?.HttpsPort > 0 ?
                    hostOptions.HttpsPort : 7001;

                options.ListenAnyIP(httpsPort, listenOpt =>
                {
#if DEBUG
                    listenOpt.UseConnectionLogging();
#endif
                    listenOpt.UseHttps(sslCert);
                    listenOpt.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                });
            }

            var httpPort = hostOptions?.HttpPort > 0 ?
                hostOptions.HttpPort : 7000;

            options.ListenAnyIP(httpPort, listenOpt =>
            {
#if DEBUG
                listenOpt.UseConnectionLogging();
#endif
                listenOpt.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
            });
        });
    }

    internal static X509Certificate2 LoadFromEnvironmentVariable()
    {
        X509Certificate2 sslCert = null!;
        var certificateData = Environment.GetEnvironmentVariable(X509_CERT);
        var certificatePassword = Environment.GetEnvironmentVariable(X509_CERT_PASS);

        if (string.IsNullOrWhiteSpace(certificateData) ||
            string.IsNullOrWhiteSpace(certificatePassword))
        {
            return sslCert;
        }

        var rawData = Convert.FromBase64String(certificateData);
        sslCert = new X509Certificate2(rawData, certificatePassword);

        return sslCert;
    }

    internal static X509Certificate2? LoadCertificate(this CertificateOptions? certificateOptions)
    {
        X509Certificate2 sslCert = null!;

        if (certificateOptions == null)
        {
            return sslCert;
        }

        // Todo could we queue off the environment OS?
        if (certificateOptions.UseWindowsStore
            && certificateOptions.CertificateStoreName.HasValue)
        {
            // You’ll sometimes see certificate stores referred to as physical or
            // logical stores. Physical stores reference the actual file system or
            // registry location where the registry key(s) and/or file(s) are stored.
            // Logical stores are dynamic references that reference one or more physical
            // stores. Logical stores are much easier to work with than physical stores
            // for most common use cases
            using var store = new X509Store((StoreName)certificateOptions.CertificateStoreName);
            store.Open(OpenFlags.ReadOnly);

            var certificateCollection = FindX509CertificateCollection(certificateOptions, store);

            if (certificateCollection == null || certificateCollection.Count == 0)
            {
                Log.Logger.Error(NO_CERTIFICATE_FOUND);
                throw new InvalidOperationException(NO_CERTIFICATE_FOUND);
            }

            return (X509Certificate2?)certificateCollection[0];
        }

        var certLocation = certificateOptions.CertificateFileLocation;

        if (!string.IsNullOrWhiteSpace(certLocation) &&
            File.Exists(certificateOptions.CertificateFileLocation))
        {
            sslCert = new(certificateOptions.CertificateFileLocation,
                certificateOptions.CertificatePassword);
        }

        return sslCert;
    }

    private static X509CertificateCollection? FindX509CertificateCollection(
        CertificateOptions certificateOptions, X509Store store)
    {
        X509CertificateCollection? certificateCollection = null;

        if (!string.IsNullOrWhiteSpace(certificateOptions.Subject))
        {
            certificateCollection = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                certificateOptions.Subject, true);
        }
        else if (!string.IsNullOrWhiteSpace(certificateOptions.SerialNumber))
        {
            certificateCollection = store.Certificates.Find(
                X509FindType.FindBySerialNumber,
                certificateOptions.SerialNumber, true);
        }
        else if (!string.IsNullOrWhiteSpace(certificateOptions.Thumbprint))
        {
            certificateCollection = store.Certificates.Find(
                X509FindType.FindByThumbprint,
                certificateOptions.Thumbprint, true);
        }

        return certificateCollection;
    }
}