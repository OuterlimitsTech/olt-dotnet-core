using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Requests;
using System;
using Xunit;

namespace OLT.EF.Core.Tests
{
    public class RequestContextTests : BaseUnitTests
    {        
        [Fact]
        [Obsolete]
        public void Tests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetRequiredService<UnitTestContext>();

                Assert.NotNull(new RequestContext(context).Context);

                var person = new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last()
                };

                var request = new RequestContextModel(context, person);
                Assert.NotNull(request.Context);
                request.Value.Should().BeEquivalentTo(person);
            }
        }
    }
}
