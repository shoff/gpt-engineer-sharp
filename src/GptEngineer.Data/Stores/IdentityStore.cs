namespace GptEngineer.Data;

using Core;
using Core.Stores;

public class IdentityStore : DataStore, IIdentityStore
{
    public IdentityStore(string path)
        : base(path)
    {
    }
}