using Microsoft.Extensions.DependencyInjection;
using OLT.Builder.File.Tests.Assets;
using OLT.Core;
using System;

namespace OLT.Builder.File.Tests
{
    public abstract class BaseUnitTests
    {

        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IOltFileBuilder, TestCsvBuilder>();
            services.AddScoped<IOltFileBuilder, TestCsvBuilderTyped>();
            services.AddScoped<IOltFileBuilder, TestCsvServiceBuilder>();
            services.AddScoped<IOltFileBuilderManager, OltFileBuilderManager>();
            
            return services.BuildServiceProvider();
        }

    }
}
