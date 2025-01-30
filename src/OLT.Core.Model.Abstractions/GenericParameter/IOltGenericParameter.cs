namespace OLT.Core
{
    public interface IOltGenericParameter
    {
        T? GetValue<T>(string key) where T : IConvertible;
        T GetValue<T>(string key, T defaultValue) where T : IConvertible;
        string GetValue(string key);
    }
}