using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Services.Tests.Assets
{
    public class TestCoreService : OltCoreService<TestServiceManager>, ITestCoreService
    {
        public TestCoreService(IOltServiceManager serviceManager) : base(serviceManager)
        {
        }

        public string ServiceManagerName => ServiceManager.ValidateName;
    }
}
