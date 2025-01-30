namespace OLT.Core;

internal static class OltInternalExtensions
{

    internal static int ToPort(this string? self, int defaultValue)
    {
        if (!int.TryParse(self, out var value))
            return defaultValue;
        return value;
    }

    internal static bool? ToBoolean(this string? self, bool? defaultValue)
    {
        if (bool.TryParse(self, out var val))
        {
            return val;
        }

        return defaultValue;
    }
}
