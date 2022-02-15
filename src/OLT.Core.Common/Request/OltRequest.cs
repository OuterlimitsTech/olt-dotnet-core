namespace OLT.Core
{


    /// <summary>
    /// Base Request object
    /// </summary>
    public abstract class OltRequest : IOltRequest
    {
        protected OltRequest()
        {
        }
    }


    /// <summary>
    /// General Request with a passed Value
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OltRequest<TValue> : OltRequest, IOltRequest<TValue>
    {
        public OltRequest(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}