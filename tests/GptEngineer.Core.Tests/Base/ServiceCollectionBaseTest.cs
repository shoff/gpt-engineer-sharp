namespace GptEngineer.Core.Tests.Base;

using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

public abstract class ServiceCollectionBaseTest
    : BaseTest, IDisposable
{
    protected IServiceCollection services;
    protected readonly CancellationTokenSource cancellationTokenSource;

    protected ServiceCollectionBaseTest()
        : this(null) { }

    protected ServiceCollectionBaseTest(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        this.services = new TestServiceCollection();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        this.cancellationTokenSource?.Dispose();

    }

    ~ServiceCollectionBaseTest()
    {
        this.Dispose(false);
    }

}