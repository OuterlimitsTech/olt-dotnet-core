using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Common.Tests.Assets
{
    public class TestDisposable : OltDisposable
    {
        public bool IsDeposed()
        {
            return base.Disposed;
        }
    }
}
