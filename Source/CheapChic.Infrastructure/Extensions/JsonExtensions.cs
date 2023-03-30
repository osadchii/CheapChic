using Newtonsoft.Json;

namespace CheapChic.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string ToJson(this object obj)
    {
        if (obj is null)
        {
            return null;
        }
        
        return JsonConvert.SerializeObject(obj);
    }

    public static TEntity FromJson<TEntity>(this string str)
    {
        return JsonConvert.DeserializeObject<TEntity>(str);
    }
}