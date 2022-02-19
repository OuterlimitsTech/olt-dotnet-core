using System;
using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
    public class OltSendGridValidationException : Exception
    {
        public OltSendGridValidationException(List<string> errors)
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
