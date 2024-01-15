namespace OLT.Core.Common.Tests.Assets;

public class TestServiceManager : OltDisposable, IOltServiceManager
{
    public string ValidateName => nameof(TestServiceManager);
}