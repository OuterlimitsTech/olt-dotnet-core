using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Services.Tests.Assets;
using Xunit;

namespace OLT.Core.Services.Tests
{
    public class GeneralTests
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<ITestCoreService, TestCoreService>();
            services.AddScoped<IOltServiceManager, TestServiceManager>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void ServiceManagerTests()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<ITestCoreService>();
                Assert.NotNull(service as TestCoreService);
                Assert.Equal(nameof(TestServiceManager), service.ServiceManagerName);                
            }
        }
    }
}