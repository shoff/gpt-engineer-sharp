using GptEngineer.Core;
using Microsoft.Extensions.Options;

namespace GptEngineer.Infrastructure.Extensions;

public static class OptionsExtensions
{
    public static void Validate<T>(this IOptions<T> options) 
        where T : class, IValidatable
    {
        options.Value?.Validate();
    }
}