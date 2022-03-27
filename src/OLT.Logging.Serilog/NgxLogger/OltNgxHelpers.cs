using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OLT.Logging.Serilog
{
    internal static class OltNgxHelpers
    {
        /// <summary>
        /// Took this method from <see href="https://josef.codes/transform-csharp-objects-to-a-flat-string-dictionary/"/>.        /// 
        /// </summary>
        /// <remarks>
        /// I could not get Implementation2 or Implementation3 to work, but it's not needed.  This method is only called when logging a frontend ngx-logger message
        /// </remarks>
        /// <param name="object"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> ConvertToDictionary(object @object, string prefix = "")
        {
            var dictionary = new Dictionary<string, string>();
            Flatten(dictionary, @object, prefix);
            return dictionary;
        }

        private static void Flatten(
            IDictionary<string, string> dictionary,
            object source,
            string name)
        {
            var properties = source.GetType().GetProperties().Where(x => x.CanRead);
            foreach (var property in properties)
            {
                var key = string.IsNullOrWhiteSpace(name) ? property.Name : $"{name}.{property.Name}";

                object value = null;
                try
                {
                    value = property.GetValue(source, null);
                }
                catch
                {
                    // Do Nothing
                }


                if (value == null)
                {
                    dictionary[key] = null;
                    continue;
                }

                if (property.PropertyType.IsValueTypeOrString())
                {
                    dictionary[key] = value.ToStringValueType();
                }
                else
                {
                    if (value is IEnumerable enumerable)
                    {
                        var counter = 0;
                        foreach (var item in enumerable)
                        {
                            var itemKey = $"{key}[{counter++}]";
                            if (item.GetType().IsValueTypeOrString())
                            {
                                dictionary.Add(itemKey, item.ToStringValueType());
                            }
                            else
                            {
                                Flatten(dictionary, item, itemKey);
                            }
                        }
                    }
                    else
                    {
                        Flatten(dictionary, value, key);
                    }
                }
            }
        }


        private static bool IsValueTypeOrString(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        private static string ToStringValueType(this object value)
        {
            if (value is DateTime)
            {
                return ((DateTime)value).ToString("o");
            }

            if (value is bool)
            {
                return ((bool)value) ? "true" : "false";
            }

            return value?.ToString();
        }

    }
}