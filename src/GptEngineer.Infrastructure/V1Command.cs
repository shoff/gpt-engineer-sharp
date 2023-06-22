namespace GptEngineer.Infrastructure;

using GptEngineer.Core;
using Mediator;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public abstract record V1Command : IRequest<MediatorMessage>, IValidatable
{
    public const string VERSION = "v1";
    public abstract void Validate();
}