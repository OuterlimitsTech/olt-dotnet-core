using System;
using System.Collections.Generic;

namespace OLT.Email
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public abstract class OltEmailValidationException : OLT.Core.OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        private OltEmailValidationException(string message) : base(message)
        {

        }

        protected OltEmailValidationException(List<string> errors, string message) : this(message)
        {
            Errors = errors;
        }

        public List<string> Errors { get;  }

        public OltEmailResult ToEmailResult()
        {
            return new OltEmailResult
            {
                Errors = Errors
            };
        }
    }
}
