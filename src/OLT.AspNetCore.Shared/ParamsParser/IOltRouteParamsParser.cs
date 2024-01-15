namespace OLT.Core
{
    public interface IOltRouteParamsParser<TValue>
    {
        bool TryParse(string? param, out TValue value);
    }



}
