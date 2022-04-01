namespace OLT.Core
{
    public interface IOltGenericParameterParser
    {
        string Key { get; }
        bool HasValue { get; }
        bool Parse(IOltGenericParameter parameters);
    }

    public interface IOltGenericParameterParser<out TValueType> : IOltGenericParameterParser
    {        
        TValueType Value { get; }
    }

    //public interface IOltGenericParameterParserWithDefault<out TValueType> : IOltGenericParameterParser<TValueType>
    //{
    //    TValueType DefaultValue { get; }
    //}
}
