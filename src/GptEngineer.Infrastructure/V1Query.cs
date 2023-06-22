namespace GptEngineer.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using Core;
using Mediator;

[ExcludeFromCodeCoverage]
public abstract record V1Query<TData>
    : IRequest<MediatorMessage<TData>>, IValidatable
{
    protected string version = "v1";

    public abstract void Validate();
}