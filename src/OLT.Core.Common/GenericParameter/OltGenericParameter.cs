using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public class OltGenericParameter : IOltGenericParameter
    {

        public OltGenericParameter(Dictionary<string, string> values)
        {
            Values = values;
        }

        public Dictionary<string, string> Values { get; }

        public T GetValue<T>(string key) where T : IConvertible
        {
            var entry = Values.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
            if (entry != null)
            {
                try
                {
                    return (T)Convert.ChangeType(entry, typeof(T));
                }
                catch
                {
                    // Do Nothing
                }
            }
            return default(T);
        }


        public T GetValue<T>(string key, T defaultValue) where T : IConvertible
        {
            var entry = Values.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
            if (entry != null)
            {
                try
                {
                    return (T)Convert.ChangeType(entry, typeof(T));
                }
                catch
                {
                    // Do Nothing
                }
                
            }
            return defaultValue;
        }

        public object GetValue(string key)
        {
            return Values.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
        }
    }
}