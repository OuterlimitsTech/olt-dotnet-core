using System;

namespace OLT.Core
{
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
