using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OLT.Core
{
    public class OltRuleException : OltException
    {

        public OltRuleException(string errorMessage, Exception ex = null) : base(errorMessage, ex)
        {

        }

    }
}