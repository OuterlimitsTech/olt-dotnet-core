using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Tests
{
    public class RequestContextTests : BaseUnitTests
    {        
        [Fact]
        public void Tests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();

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
