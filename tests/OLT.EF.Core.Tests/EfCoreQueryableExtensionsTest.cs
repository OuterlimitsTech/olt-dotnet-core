using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Tests
{

    public class EfCoreQueryableExtensionsTest : BaseUnitTests
    {
        [Fact]
        public async Task ToPagedAsyncTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                if (context == null) throw new Exception("Missing Context");
                await context.People.AddRangeAsync(PersonEntity.FakerList(35));
                await context.SaveChangesAsync();

                var queryable = context.People.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst);

                var @params = new OltPagingParams
                {
                    Page = 1,
                    Size = 10
                };

                var paged = await OltEfCoreQueryableExtensions.ToPagedAsync(queryable, @params);

                Assert.Equal(@params.Page, paged.Page);
                Assert.Equal(@params.Size, paged.Size);
                Assert.Equal(paged.Count, paged.Count);
                Assert.Equal(@params.Size, paged.Data.Count());

                await Assert.ThrowsAsync<ArgumentNullException>("queryable", () => OltEfCoreQueryableExtensions.ToPagedAsync<PersonEntity>(null, @params));
                await Assert.ThrowsAsync<ArgumentNullException>("pagingParams", () => OltEfCoreQueryableExtensions.ToPagedAsync(queryable, null));
            }
        }

    }
}