using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;

namespace OLT.AspNetCore.Tests
{
    public class FakeValueProvider : IValueProvider
    {
        readonly string _key;
        readonly string _value;

        public FakeValueProvider()
        {
            
        }

        public FakeValueProvider(string key, string value)
        {
            _key = key;
            _value = value;
        }
        public bool ContainsPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public ValueProviderResult GetValue(string key)
        {
            return new ValueProviderResult(new StringValues(_value));
        }
    }
}
