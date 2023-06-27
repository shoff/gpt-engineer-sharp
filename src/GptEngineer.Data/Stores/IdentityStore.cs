namespace GptEngineer.Data.Stores;

using Core.Configuration;
using Core.Stores;
using Microsoft.Extensions.Options;

public class IdentityStore : DataStore, IIdentityStore
{
    public IdentityStore(IOptions<IdentityOptions> options)
        : base(options.Value.Path)
    {
    }
}