using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OLT.Core.Common.Tests.Assets
{
    public class TestIdentity : OltIdentity
    {

        public TestIdentity()
        {
        }

        public TestIdentity(GenericIdentity identity)
        {
            Identity = new GenericPrincipal(identity, null);
        }

        public TestIdentity(GenericIdentity identity, string[] roles)
        {
            Identity = new GenericPrincipal(identity, roles);
        }

        public override ClaimsPrincipal Identity { get; }
    }
}
