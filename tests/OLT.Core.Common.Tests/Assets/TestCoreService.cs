namespace OLT.Core.Common.Tests.Assets;

public class TestCoreService : OltCoreService<TestServiceManager>, ITestCoreService
{
    public TestCoreService(IOltServiceManager serviceManager) : base(serviceManager)
    {
    }

    public string ServiceManagerName => ServiceManager.ValidateName;
}