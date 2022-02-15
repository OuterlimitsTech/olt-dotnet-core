namespace OLT.Core
{

    /// <summary>
    /// General Request with a passed Value
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OltRequest<TValue> : IOltRequest<TValue>
    {
        public OltRequest(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}