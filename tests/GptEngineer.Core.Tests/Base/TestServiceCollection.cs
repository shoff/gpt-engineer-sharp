namespace GptEngineer.Core.Tests.Base;

using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

public class TestServiceCollection : ServiceCollection, IServiceCollection
{
    public TestServiceCollection(
        ITestOutputHelper outputHelper = null!, Microsoft.Extensions.Logging.LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Debug)
    {
        this.Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        this.AddSingleton(this.Configuration);
        this.AddRouting();
        this.AddOptions();
        this.AddLogging(outputHelper);
        this.RegisterLoggers();
        this.AddHttpClient();
    }

    public IConfiguration Configuration { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public T GetRequiredService<T>()
    {
        var sp = this.BuildServiceProvider();
        return sp.GetRequiredService<T>();
    }

    private void RegisterLoggers()
    {
        this.AddSingleton(new Mock<ILogger>().Object);
    }
}