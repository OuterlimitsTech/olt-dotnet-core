using System.ComponentModel;

namespace OLT.Core
{
    internal static class OltInteralExtensions
    {

        internal static T? GetAttributeInstanceInternal<T>(this Enum item)
            where T : Attribute
        {
            if (item == null) return null;
            var type = item.GetType();
            var attribute = type.GetField(item.ToString())?.GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();
            return attribute;
        }

        internal static string? GetDescriptionInternal(this Enum value)
        {
            var attribute = GetAttributeInstanceInternal<DescriptionAttribute>(value);
            return attribute?.Description ?? value?.ToString();
        }

    }

}
