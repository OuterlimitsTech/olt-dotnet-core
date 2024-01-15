namespace OLT.Core
{

    public abstract class OltGenericParameterParser<TValueType> : IOltGenericParameterParser<TValueType>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected OltGenericParameterParser(string key)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Key = key;
        }

        public virtual string Key { get; } = default!;
        public abstract bool HasValue { get; }
        public abstract bool Parse(IOltGenericParameter parameters);
        public virtual TValueType Value { get; set; }
    }
}
