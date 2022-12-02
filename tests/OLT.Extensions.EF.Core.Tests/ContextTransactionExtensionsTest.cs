using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Extensions.EF.Core.Tests.Assets;
using OLT.Extensions.EF.Core.Tests.Assets.Entites;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.EF.Core.Tests
{

    public class OltEntityFrameworkCoreExtensionsTest : BaseUnitTests
    {
        [Fact]
        public async Task CreateSubTransactionTests()
        {
            using (var provider = BuildProvider())
            {
                using (var tran = new MockTran())
                {
                    await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(tran, () =>
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
                        await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(tran, () => { throw new System.Exception("Test"); });
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
                    await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync<UserEntity>(tran, () =>
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
                        await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync<UserEntity>(tran, () => { throw new System.Exception("Test"); });
                        Assert.True(false);
                    }
                    catch
                    {
                        Assert.True(true);
                    }

                }
            }
        }

        [Fact]
        public async Task UsingDbTransactionTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync(context.Database, async () =>
                {
                    await SubTran(context);
                });
            }


            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                try
                {
                    await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync(context.Database, () => { throw new System.Exception("Test"); });
                    Assert.True(false);
                }
                catch
                {
                    Assert.True(true);
                }

            }


            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();

                await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync<UserEntity>(context.Database, async () =>
                {
                    return await UserEntityTran(context);
                });
            }


            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                try
                {
                    await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync<UserEntity>(context.Database, () => { throw new System.Exception("Test"); });
                    Assert.True(false);
                }
                catch
                {
                    Assert.True(true);
                }
            }
        }

        private async Task SubTran(UnitTestContext context)
        {
            await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync(context.Database, () =>
            {
                return Task.CompletedTask;
            });
        }

        private async Task<UserEntity> UserEntityTran(UnitTestContext context)
        {
            return await OltEntityFrameworkCoreExtensions.UsingDbTransactionAsync<UserEntity>(context.Database, () =>
            {
                return Task.FromResult(new UserEntity());
            });
        }
    }
}