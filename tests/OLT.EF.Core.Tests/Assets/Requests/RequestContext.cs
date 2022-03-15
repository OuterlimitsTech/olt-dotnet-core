using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets.Requests
{
    public class RequestContext : OltRequestContext<UnitTestContext>
    {
        public RequestContext(UnitTestContext context) : base(context)
        {
        }
    }

    public class RequestContextModel : OltRequestContext<UnitTestContext, OltPersonName>
    {
        public RequestContextModel(UnitTestContext context, OltPersonName value) : base(context, value)
        {
        }
    }
}
