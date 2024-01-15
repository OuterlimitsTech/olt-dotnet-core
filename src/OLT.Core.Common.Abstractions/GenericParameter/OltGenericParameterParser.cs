namespace OLT.Core
{

    public abstract class OltGenericParameterParser<TValueType> : IOltGenericParameterParser<TValueType>
    {
        protected OltGenericParameterParser(string key)
        {
            Key = key;
        }

        public virtual string Key { get; } = default!;
        public abstract bool HasValue { get; }
        public abstract bool Parse(IOltGenericParameter parameters);
        public virtual TValueType Value { get; set; }
    }
}
