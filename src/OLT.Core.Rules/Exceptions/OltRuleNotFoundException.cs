using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OLT.Core
{
    public class OltRuleNotFoundException : OltException
    {

        public OltRuleNotFoundException(string message) : base(message)
        {
            
        }

        public OltRuleNotFoundException(Type ruleType) : base($"{ruleType.FullName} rule not found")
        {

        }
    }
}