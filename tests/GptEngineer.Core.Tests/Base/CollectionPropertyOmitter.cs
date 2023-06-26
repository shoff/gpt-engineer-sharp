namespace GptEngineer.Core.Tests.Base;

using System.Reflection;
using AutoFixture.Kernel;

public class CollectionPropertyOmitter : ISpecimenBuilder
{
    /// <summary>
    /// Internal handling of the ignore collection properties
    /// customization.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public object Create(object request, ISpecimenContext context)
    {
        var pi = request as PropertyInfo;
        if (pi != null
            && pi.PropertyType.IsGenericType
            && pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
        {
            return new OmitSpecimen();
        }

        return new NoSpecimen();
    }
}