using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OLT.Core.DependencyInjection.Abstractions.Tests
{
    public class OltDependencyInjectionExtensionsTests
    {
        [Fact]
        public void ScanAssemblies_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            services.AddServicesFromAssemblies(builder =>
            {
                builder.IncludeAssembly(assembly);
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.Equal(3, serviceProvider.GetServices<IOltInjectableScoped>().Count());
            Assert.Single(serviceProvider.GetServices<IOltInjectableSingleton>());
            Assert.Single(serviceProvider.GetServices<IOltInjectableTransient>());


        }


        [Fact]
        public void ScanAssemblies_UsingBuilder_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var assembly = Assembly.GetExecutingAssembly();
            var hostBuilder = new OltHostBuilder(services);

            // Act
            hostBuilder.AddServicesFromAssemblies(builder =>
            {
                builder.IncludeAssembly(assembly);
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var registeredServices = serviceProvider.GetServices<IOltInjectableScoped>().ToList();
            //Assert.NotEmpty(registeredServices);
            Assert.Equal(3, serviceProvider.GetServices<IOltInjectableScoped>().Count());
            Assert.Single(serviceProvider.GetServices<IOltInjectableSingleton>());
            Assert.Single(serviceProvider.GetServices<IOltInjectableTransient>());
        }



        public class ScopedObject1 : OltDisposable, IOltInjectableScoped
        {

        }

        public class ScopedObject2 : OltDisposable, IOltInjectableScoped
        {

        }

        public class ScopedObject3 : OltDisposable, IOltInjectableScoped
        {

        }

        public class SingletonObject : OltDisposable, IOltInjectableSingleton
        {

        }

        public class TransientObject : OltDisposable, IOltInjectableTransient
        {

        }
    }
}