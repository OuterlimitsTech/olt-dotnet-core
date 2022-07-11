namespace OLT.Core
{
    public interface IOltGenericFilterTemplate : IOltGenericFilter
    {
        IOltFilterTemplate FilterTemplate { get; }
    }
}
