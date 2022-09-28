using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Tests
{
    public class ContextTransactionExtensionsTest : BaseUnitTests
    {
        [Fact]
        public async Task CreateSubTransactionTests()
        {
            using (var provider = BuildProvider())
            {
                using (var tran = new MockTran())
                {
                    await OltContextTransactionExtensions.CreateSubTransactionAsync(tran, () =>
                    {
                        return Task.CompletedTask;
                    });
                }
            }


            using (var provider = BuildProvider())
            {
                using (var tran = new MockTran())
                {                    
                    try
                    {
                        await OltContextTransactionExtensions.CreateSubTransactionAsync(tran, () => { throw new System.Exception("Test"); });
                        Assert.True(false);
                    }
                    catch
                    {
                        Assert.True(true);
                    }
                    
                }
            }


            using (var provider = BuildProvider())
            {
                using (var tran = new MockTran())
                {
                    await OltContextTransactionExtensions.CreateSubTransactionAsync<UserEntity>(tran, () =>
                    {
                        return Task.FromResult(new UserEntity());
                    });
                }
            }


            using (var provider = BuildProvider())
            {
                using (var tran = new MockTran())
                {
                    try
                    {
                        await OltContextTransactionExtensions.CreateSubTransactionAsync<UserEntity>(tran, () => { throw new System.Exception("Test"); });
                        Assert.True(false);
                    }
                    catch
                    {
                        Assert.True(true);
                    }

                }
            }
        }
    }
}