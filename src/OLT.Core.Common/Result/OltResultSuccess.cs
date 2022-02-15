namespace OLT.Core
{
    /// <summary>
    /// General Success Result
    /// </summary>
    public class OltResultSuccess : IOltResult
    {
        public virtual bool Success => true;
    }
}