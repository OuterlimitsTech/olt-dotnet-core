using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject3ToAdapterObject5AfterMap : OltAdapterAfterMap<AdapterObject3, AdapterObject5>
    {
        private readonly string _value;

        public AdapterObject3ToAdapterObject5AfterMap(string value)
        {
            _value = value;
        }

        
        public override IQueryable<AdapterObject5> AfterMap(IQueryable<AdapterObject5> queryable)
        {
            return queryable.Where(p => p.FirstName.Contains(_value));
        }
    }
}
