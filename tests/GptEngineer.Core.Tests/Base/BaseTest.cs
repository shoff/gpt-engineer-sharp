namespace GptEngineer.Core.Tests.Base;


using System.Linq;
using AutoFixture;
using Xunit.Abstractions;

/// <summary>
/// The most basic test class, with inversion of control function
/// </summary>
public abstract class BaseTest
{
    protected readonly ITestOutputHelper outputHelper;

    /// <summary>
    /// AutoFixture is an open source library for .NET designed to minimize the 'Arrange'
    /// phase of your unit tests in order to maximize maintainability. Its primary goal is to
    /// allow developers to focus on what is being tested rather than how to setup the test
    /// scenario, by making it easier to create object graphs containing test data.
    /// </summary>
    /// <remarks>
    /// Git hub: <see href="https://github.com/AutoFixture/AutoFixture"/>
    /// Pluralsight: <see href="https://app.pluralsight.com/library/courses/autofixture-dotnet-unit-test-get-started/table-of-contents"/>
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    protected readonly IFixture fixture;

    protected BaseTest(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
        fixture = new Fixture();
        fixture.Customize(new DoNotFillCollectionProperties());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}

// See http://blog.ploeh.dk/2011/03/18/EncapsulatingAutoFixtureCustomizations/