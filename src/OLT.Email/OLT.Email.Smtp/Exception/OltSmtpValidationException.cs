using System;
using System.Collections.Generic;

namespace OLT.Email.Smtp
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltSmtpValidationException : OLT.Email.OltEmailValidationException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public OltSmtpValidationException(List<string> errors) : base(errors, "Smtp Validation")
        {
        }

    }
}
