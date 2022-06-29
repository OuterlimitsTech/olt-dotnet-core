// ReSharper disable once CheckNamespace
namespace OLT.Core
{
    /// <summary>
    /// Used to define serializer for distrubuted cache provider like Redis
    /// </summary>
    public interface IOltCacheSerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string value);
    }

}