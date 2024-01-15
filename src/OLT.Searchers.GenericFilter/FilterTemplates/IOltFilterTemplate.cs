namespace OLT.Core
{
    public interface IOltFilterTemplate : IOltTemplate, IOltGenericParameterParser
    {
        string Label { get; }
        bool Hidden { get; }
        string? Formatted();
    }

    public interface IOltFilterTemplate<out TValueType> : IOltFilterTemplate, IOltGenericParameterParser<TValueType>
    {        
        
    }

}
