namespace OLT.Core.Services.Tests.Assets;

public interface ITestCoreService : IOltCoreService
{
    string ServiceManagerName { get; }
}