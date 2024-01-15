namespace OLT.Core.Common.Tests.Assets;

public class TestDisposable : OltDisposable
{
    public bool IsDisposed()
    {
        return base.Disposed;
    }
}