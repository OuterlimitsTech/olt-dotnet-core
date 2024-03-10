using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets.Requests
{
    [Obsolete]
    public class RequestContext : OltRequestContext<UnitTestContext>
    {
        public RequestContext(UnitTestContext context) : base(context)
        {
        }
    }

    [Obsolete]
    public class RequestContextModel : OltRequestContext<UnitTestContext, OltPersonName>
    {
        public RequestContextModel(UnitTestContext context, OltPersonName value) : base(context, value)
        {
        }
    }
}
