namespace GptEngineer.Infrastructure.Extensions;

using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

public static class CacheExtensions
{
    public static bool TryGetFromCache<T>(this IDistributedCache cache, string key, out T? t)
    {
        try
        {
            var data = cache.Get(key);

            if (data == null)
            {
                t = default;
                return false;
            }

            var serializedString = Encoding.UTF8.GetString(data);
            if (!string.IsNullOrWhiteSpace(serializedString))
            {
                t = JsonSerializer.Deserialize<T>(serializedString) ?? default;
                return true;
            }
        }
        catch (Exception e)
        {
            // TODO we need to notify the caller that something went wrong
        }
        t = default;
        return false;
    }

    public static bool TryAddToCache<T>(this IDistributedCache cache,
        string key, T value, DistributedCacheEntryOptions? options = null)
    {
        try
        {
            var serializedObject = JsonSerializer.Serialize(value);
            var data = Encoding.UTF8.GetBytes(serializedObject);

            if (options != null)
            {
                cache.Set(key, data, options);
            }
            else
            {
                cache.Set(key, data);
            }

            return true;
        }
        catch (Exception e)
        {
            // TODO we need to notify the caller that something went wrong
        }
        return false;
    }
}