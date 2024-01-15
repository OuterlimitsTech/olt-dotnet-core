namespace OLT.Core
{
    public abstract class OltRouteParamsParser<TValue> : IOltRouteParamsParser<TValue>
    {
        public abstract bool TryParse(string? param, out TValue value);
    }



}
