namespace OLT.Core
{
    /// <summary>
    /// General Exception
    /// </summary>
    /// <remarks>
    /// Using <seealso cref="Exception"/> a code smell
    /// </remarks>
    public class OltException : Exception
    {

        public OltException(string message) : base(message)
        {

        }

        public OltException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }

}
