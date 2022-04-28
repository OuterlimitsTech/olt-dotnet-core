namespace OLT.Core.Services.Tests.Assets
{
    public class TestServiceManager : OltDisposable, IOltServiceManager
    {
        public string ValidateName => nameof(TestServiceManager);
    }
}
