namespace GptEngineer.Core.Tests.Base;

using AutoFixture;

public class DoNotFillCollectionProperties : ICustomization
{
    /// <inheritdoc />
    /// <summary>
    /// Customizes the specified fixture so that it ignores generic collection properties
    /// </summary>
    /// <param name="fixture">The fixture to customize.</param>
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new CollectionPropertyOmitter());
    }
}