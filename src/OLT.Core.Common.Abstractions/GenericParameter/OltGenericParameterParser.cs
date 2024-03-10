using System.Linq;

namespace OLT.Core
{

    public abstract class OltGenericParameterParser<TValueType> : IOltGenericParameterParser<TValueType>
    {
        protected OltGenericParameterParser(string key)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(key);
#else
            OltArgumentNullException.ThrowIfNull(key, nameof(key));
            OltArgumentNullException.ThrowIfNull(key, nameof(key));
#endif

            Key = key;
        }

        public virtual string Key { get; } = default!;
        public abstract bool HasValue { get; }
        public abstract bool Parse(IOltGenericParameter parameters);
        public virtual TValueType Value { get; set; } = default!;
    }
}
