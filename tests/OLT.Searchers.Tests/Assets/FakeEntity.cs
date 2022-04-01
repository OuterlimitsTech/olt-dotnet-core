using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Searchers.Tests.Assets
{
    public class FakeEntity : IOltEntity
    {
        public DateTimeOffset SomeDate { get; set; }
    }
}
