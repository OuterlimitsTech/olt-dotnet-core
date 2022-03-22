using OLT.Core.Common.Tests.Assets;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class HostServiceTests
    {

        [Fact]
        public void ModelTests()
        {
            var envName = Faker.Lorem.Words(10).Last();
            var appName = Faker.Lorem.Words(15).Last();
            var dir = $"/{envName}/{appName}/";
            var filePath = "~/test";
            var expected = filePath.Replace("~/", dir);

            var hostModel = new TestHostService(envName, appName);

            Assert.Equal(envName, hostModel.EnvironmentName);
            Assert.Equal(appName, hostModel.ApplicationName);
            var val = hostModel.ResolveRelativePath(filePath);
            Assert.Equal(expected, val);
        }

    }
}