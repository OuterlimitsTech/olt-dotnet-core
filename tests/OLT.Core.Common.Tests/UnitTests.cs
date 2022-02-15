using OLT.Core.Common.Tests.Assets;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class UnitTests
    {
        [Fact]
        public void OltDisposable()
        {
            using(var obj = new TestDisposable())
            {
                Assert.NotNull(obj);
                Assert.False(obj.IsDeposed());
                obj.Dispose();
                Assert.True(obj.IsDeposed());
            }
        }
    }
}