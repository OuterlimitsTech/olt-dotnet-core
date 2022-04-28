using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public abstract class OltRuleBuilderWithService<T> : OltRuleBuilderWithParameter<T> 
        where T : OltRuleBuilderWithService<T>
    {

        private readonly Dictionary<string, IOltCoreService> _services = new Dictionary<string, IOltCoreService>();

        public T WithService<TService>(TService service) where TService : class, IOltCoreService
        {
            var fullName = service.GetType().FullName;
            if (!_services.ContainsKey(fullName))
            {
                _services.Add(fullName, service);
            }
            return (T)this;
        }

        public TService GetService<TService>() where TService : class, IOltCoreService
        {
            var result = _services.FirstOrDefault(p => p.Value.GetType().Implements<TService>());
            if (result.Value == null)
            {
                throw new OltRuleMissingServiceException<TService>(this);
            }
            return result.Value as TService;
        }
    }
}
