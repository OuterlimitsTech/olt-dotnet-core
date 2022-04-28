using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public abstract class OltRuleBuilderWithParameter<T> : OltRuleBuilderBase
        where T : OltRuleBuilderWithParameter<T>
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public T WithParameter<TData>(TData data) where TData : class
        {
            var fullName = data.GetType().FullName;
            if (!_values.ContainsKey(fullName))
            {
                _values.Add(fullName, data);
            }
            return (T)this;
        }

        public TParameter GetParameter<TParameter>() where TParameter : class
        {
            var fullName = typeof(TParameter).FullName;
            var result = _values.FirstOrDefault(p => p.Key == fullName);
            if (result.Value == null)
            {
                throw new OltRuleMissingParameterException<TParameter>(this);
            }
            return result.Value as TParameter;
        }
    }
}
