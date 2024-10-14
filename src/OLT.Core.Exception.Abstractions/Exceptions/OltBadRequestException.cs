namespace OLT.Core
{
    /// <summary>
    /// Cannot process a request due to an error
    /// </summary>
    public class OltBadRequestException : OltException
    {
        public OltBadRequestException(string message) : base(message)
        {

        }
    }
}