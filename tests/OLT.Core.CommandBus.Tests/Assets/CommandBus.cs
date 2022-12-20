using OLT.Core.CommandBus.Tests.Assets.EfCore;
using System.Collections.Generic;

namespace OLT.Core.CommandBus.Tests.Assets
{
    public class UnitTestCommandBus : OltCommandBus<UnitTestContext>
    {
        public UnitTestCommandBus(IEnumerable<IOltCommandHandler> handlers, UnitTestContext context) : base(handlers, context)
        {
        }
    }
}
