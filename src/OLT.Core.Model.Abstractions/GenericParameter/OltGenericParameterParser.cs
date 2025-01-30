namespace OLT.Core
{

    public abstract class OltGenericParameterParser<TValueType> : IOltGenericParameterParser<TValueType>
    {
        protected OltGenericParameterParser(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(key);

            Key = key;
        }

        public virtual string Key { get; }
        public abstract bool HasValue { get; }
        public abstract bool Parse(IOltGenericParameter parameters);
        public virtual TValueType Value { get; set; } = default!;
    }
}
