namespace OLT.Core
{
    public interface IOltFilterTemplate : IOltTemplate, IOltGenericParameterParser
    {
        string Label { get; }
        bool Hidden { get; }
        string ToString();
    }

    public interface IOltFilterTemplate<out TValueType> : IOltFilterTemplate
    {        
        TValueType Value { get; }
    }

}
