using System;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public class OltRuleServiceManager : OltDisposable, IOltRuleServiceManager
    {
        private readonly IServiceProvider _serviceProvider;

        public OltRuleServiceManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TService GetService<TService>() where TService : notnull
        {
            return _serviceProvider.GetRequiredService<TService>();
        }
    }
}